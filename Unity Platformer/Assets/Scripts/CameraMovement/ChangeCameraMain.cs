using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeCameraMain : MonoBehaviour
{
    private CinemachineVirtualCamera _vCamZoomed;
    private CinemachineVirtualCamera _vCamMain;

    private void Start()
    {
        _vCamZoomed = GameObject.FindGameObjectWithTag("VCamZoom").GetComponent<CinemachineVirtualCamera>();
        _vCamMain = GameObject.FindGameObjectWithTag("VCamMain").GetComponent<CinemachineVirtualCamera>();
    }

    // Use this for initialization
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            // Make main camera highest priority vcam
            _vCamZoomed.Priority = _vCamMain.Priority - 1;
            Debug.Log("Zooming in camera");
        }
    }
}
