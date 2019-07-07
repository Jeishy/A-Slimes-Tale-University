using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class StartCutscene : MonoBehaviour
{
    [SerializeField] private HunterMovement _hunterMovement;
    [SerializeField] private float _custscenePlayTime;
    [SerializeField] private CinemachineVirtualCamera _mainCamera, _farCamera;
    [SerializeField] private Transform _hunterTrans;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        _hunterMovement.DisableMovement();
        yield return StartCoroutine(StartCustscene());
        _hunterMovement.EnableMovement();
    }

    private IEnumerator StartCustscene()
    {
        _mainCamera.m_Follow = null;
        _farCamera.m_Follow = null;
        yield return new WaitForSeconds(_custscenePlayTime);
        _mainCamera.m_Follow = _hunterTrans;
        _farCamera.m_Follow = _hunterTrans;
    }
}
