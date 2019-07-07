using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDown : MonoBehaviour
{
    [SerializeField] private HunterMovement _hunterMovement;
    [SerializeField] private CharacterController _charController;
    [SerializeField] private Transform _hunterTrans;
    [SerializeField] private Transform _slipPosTrans;

    private Vector3 refVel = Vector3.zero;

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
        Vector3 slipPos = _slipPosTrans.position;
        float time = 0f;
        while (time <= 0.8f)
        {
            time += Time.deltaTime;
            _hunterTrans.position = Vector3.SmoothDamp(_hunterTrans.position, slipPos, ref refVel, 0.4f);
            yield return null;
        }
        _charController.enabled = true;
    }
}
