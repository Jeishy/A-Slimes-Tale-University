using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeCameraZoomOut : MonoBehaviour
{
    private CinemachineVirtualCamera vCamZoomed;
    private CinemachineVirtualCamera vCamMain;

    private void Start()
    {
        vCamZoomed = GameObject.FindGameObjectWithTag("VCamZoom").GetComponent<CinemachineVirtualCamera>();
        vCamMain = GameObject.FindGameObjectWithTag("VCamMain").GetComponent<CinemachineVirtualCamera>();
    }

    // Use this for initialization
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            // Make zoomed camera highest priority vcam
            vCamZoomed.Priority = vCamMain.Priority + 1;
            Debug.Log("Zooming out camera");
        }
    }
}
