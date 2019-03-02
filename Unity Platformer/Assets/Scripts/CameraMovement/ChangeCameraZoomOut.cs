using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeCameraZoomOut : MonoBehaviour
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
        Debug.Log("Trigger entered");
        if (col.CompareTag("Player"))
        {
            Debug.Log("Camera zoom trigger entered");
            // Make zoomed camera highest priority vcam
            _vCamZoomed.Priority = _vCamMain.Priority + 1;
            Debug.Log("Zooming out camera");
        }
    }
}
