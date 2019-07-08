using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDown : MonoBehaviour
{
    [SerializeField] private HunterMovement _hunterMovement;
    [SerializeField] private CharacterController _charController;
    [SerializeField] private Transform _hunterTrans;
    [SerializeField] private Transform[] _slipPosTrans;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Hunter"))
        {
            _hunterMovement.Trip();
            StartCoroutine(Slip());
        }
    }

    private IEnumerator Slip()
    {
        _charController.enabled = false;
        _hunterMovement.enabled = false;
        foreach (Transform slipTrans in _slipPosTrans)
        {
            Vector3 slipPos = slipTrans.position;
            float time = 0f;
            bool _isPointReached = false;
            while (time <= 1.4f && !_isPointReached)
            {
                time += Time.deltaTime;
                _hunterTrans.position = Vector3.MoveTowards(_hunterTrans.position, slipPos, 0.27f);
                float distance = Vector3.Distance(_hunterTrans.position, slipPos);
                if (Mathf.Abs(distance) < 0.05f)
                    _isPointReached = true;
                yield return null;
            }
        }
        _hunterMovement.enabled = true;
        _hunterMovement._anim.SetTrigger("Fallen");
        _charController.enabled = true;
    }
}
