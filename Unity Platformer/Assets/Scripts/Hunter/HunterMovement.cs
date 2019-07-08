using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class HunterMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _charController;
    public Animator _anim;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _gravity;
    [SerializeField] private float _maxFlipTime;
    
    private float _flipTime;
    private Vector3 _moveDirection = Vector3.zero;
    private bool _wasFacingRight = true;
    private bool _canMove = true;
    private bool _hasTurned = false;
    public CinemachineVirtualCamera _highestPriorityVCam;
    private CinemachineVirtualCamera[] _virtualCams;
    private float _speed;

    private void Start()
    {
        _flipTime = 0f;
        FindHighestPriorityCamera();
    }

    // Update is called once per frame
    void Update()
    {
        if (_charController.isGrounded && _canMove)
        {
            // Horizontal movement dependant on active cinemachine virtual camera
            Vector3 cameraRightDir = _highestPriorityVCam.transform.right;
            _moveDirection = cameraRightDir * Input.GetAxis("Horizontal") * _movementSpeed;
            _speed = Mathf.Sqrt(Mathf.Pow(_charController.velocity.x, 2) + Mathf.Pow(_charController.velocity.z, 2));
            if (Input.GetAxis("Horizontal") < 0f)
                _speed *= -1f;

            if (_wasFacingRight && _speed < -0.1  && !_hasTurned)   
            {
                Debug.Log("Flip Left");
                StartCoroutine(Flip());
            }
            else if (!_wasFacingRight && _speed > 0.1 && !_hasTurned)
            {
                Debug.Log("Flip Right");
                StartCoroutine(Flip());
            }
        }
        
        /*else if (_charController.isGrounded && !_canMove)
        {
            AutomatedWalk();
        }*/

        if (_speed > 0.1 || _speed < -0.1)
            _anim.SetBool("Moving", true);
        else
            _anim.SetBool("Moving", false);

        Debug.Log(_speed);
        _moveDirection.y -= _gravity * Time.deltaTime;
        _charController.Move(_moveDirection * Time.deltaTime);
    }

    public IEnumerator AutomatedWalkTimed(float t)
    {
        Vector3 cameraRightDir = _highestPriorityVCam.transform.right;
        float time = 0f;
        while (time <= t)
        {
            time += Time.deltaTime;
            Debug.Log("automated walk");
            _moveDirection = cameraRightDir * _movementSpeed;
            yield return null;
        }
    }

    public void FindHighestPriorityCamera()
    {
        _virtualCams = FindObjectsOfType<CinemachineVirtualCamera>();
        float highestPriority = 0;
        foreach (CinemachineVirtualCamera vCam in _virtualCams)
        {
            float priority = vCam.m_Priority;
            if (priority > highestPriority)
            {
                highestPriority = vCam.m_Priority;
                _highestPriorityVCam = vCam;
            }          
        }
    }

    public void DisableMovement()
    {
        _canMove = false;
        _speed = 0f;
        _moveDirection = Vector3.zero;
        _charController.Move(_moveDirection);
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
        DisableMovement();
        Quaternion lastRot = transform.rotation;
        Quaternion newRot = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0.0f, -180f, 0.0f));
        while (_flipTime <= _maxFlipTime)
        {
            _flipTime += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(lastRot, newRot, _flipTime);
            yield return null;
        }
        _flipTime = 0f;
        EnableMovement();
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
