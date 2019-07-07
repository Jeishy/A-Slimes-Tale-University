using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _charController;
    [SerializeField] private Animator _anim;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _gravity;
    [SerializeField] private float _maxFlipTime;
    
    private float _flipTime;
    private Vector3 _moveDirection = Vector3.zero;
    private bool _wasFacingRight = true;
    private bool _canMove = true;
    private bool _hasTurned = false;

    private void Start()
    {
        _flipTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_charController.isGrounded && _canMove)
        {
            // Horizontal movement only
            _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f);
            _moveDirection *= _movementSpeed;
            if (_wasFacingRight && _moveDirection.x < -0.1 && !_hasTurned)
            {
                Debug.Log("Flip Left");
                StartCoroutine(Flip());
            }
            else if (!_wasFacingRight && _moveDirection.x > 0.1 && !_hasTurned)
            {
                Debug.Log("Flip Right");
                StartCoroutine(Flip());
            }
            
        }
        /*else if (_charController.isGrounded && !_canMove)
        {
            AutomatedWalk();
        }*/

        if (_moveDirection.x > 0.1 || _moveDirection.x < -0.1)
            _anim.SetBool("Moving", true);
        else
            _anim.SetBool("Moving", false);
        _moveDirection.y -= _gravity * Time.deltaTime;
        _charController.Move(_moveDirection * Time.deltaTime);
    }

    private void AutomatedWalk()
    {
        _moveDirection = new Vector3(1.0f, 0.0f, 0.0f);
        _moveDirection *= _movementSpeed;
    }

    public void DisableMovement()
    {
        _canMove = false;
    }

    public IEnumerator DisableMovementTimed(float time)
    {
        _canMove = false;
        yield return new WaitForSeconds(time);
        _canMove = true;
    }

    public IEnumerator Flip()
    {
        _hasTurned = true;
        _wasFacingRight = !_wasFacingRight;
        Quaternion lastRot = transform.rotation;
        Quaternion newRot = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0.0f, -180f, 0.0f));
        while (_flipTime <= _maxFlipTime)
        {
            _flipTime += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(lastRot, newRot, _flipTime);
            yield return null;
        }
        _flipTime = 0f;
        _hasTurned = false;
    }

    public void EnableMovement()
    {
        _canMove = true;
    }

    public void Trip()
    {
        Debug.Log("Tripping");
        _anim.SetTrigger("Fall");
    }
}
