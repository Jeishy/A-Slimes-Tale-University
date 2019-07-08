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
    [SerializeField] private GameObject _caveInvisWall;

    private HunterMovement _hunterMovement;

    private void Start()
    {
        _hunterMovement = _hunterTrans.gameObject.GetComponent<HunterMovement>();
        _caveInvisWall.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Hunter"))
        {
            _charController.enabled = false;
            StartCoroutine(CaveSetup());
        }
    }

    private IEnumerator CaveSetup()
    {
        _hunterMovement.DisableMovement();
         _vCamCave.Priority = 15;
        _hunterMovement.FindHighestPriorityCamera();
        StartCoroutine(_hunterMovement.AutomatedWalkTimed(2f));
        Quaternion lastRot = _hunterTrans.rotation;
        Quaternion newRot = Quaternion.Euler(_hunterTrans.rotation.eulerAngles + new Vector3(0.0f, -90f, 0.0f));
        float time = 0f;
        while (time <= 1)
        {
            time += Time.deltaTime;
            _hunterTrans.rotation = Quaternion.Slerp(lastRot, newRot, time);
            yield return null;
        }
        _charController.enabled = true;
        _hunterMovement.EnableMovement();
        _vCamMain.m_Follow = null;
        _vCamZoomed.m_Follow = null;
        _vCamCave.m_Follow = _hunterTrans;
        yield return new WaitForSeconds(1f);
        _caveInvisWall.SetActive(true);
    }
}
