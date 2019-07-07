using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EnterCave : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _vCamMain;
    [SerializeField] private CinemachineVirtualCamera _vCamZoomed;
    [SerializeField] private CinemachineVirtualCamera _vCamCave;
    [SerializeField] private CharacterController _charController;
    [SerializeField] private Transform _hunterTrans;
    [SerializeField] private Transform _hunterMovePointTrans;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Hunter"))
        {
            _vCamMain.m_Follow = null;
            _vCamZoomed.m_Follow = null;
            StartCoroutine(MoveHunter());
        }
    }

    private IEnumerator MoveHunter()
    {
        _vCamMain.Priority = _vCamCave.Priority - 1;
        _charController.enabled = false;
        Vector3 lastHunterPos = _hunterTrans.position;
        Vector3 newHunterPos = _hunterMovePointTrans.position;
        float time = 0f;
        while (time <= 0.5)
        {
            time += Time.deltaTime;
            _hunterTrans.position = Vector3.Lerp(lastHunterPos, newHunterPos, time);
            yield return null;
        }
        _charController.enabled = true;
        _vCamCave.m_Follow = _hunterTrans;
    }
}
