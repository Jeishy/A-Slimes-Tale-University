using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUbWorldDoorOverAndExit : MonoBehaviour {

    private HubWorldManager _hubWorldManager;
    private bool _isDoorOver;

    private void OnEnable()
    {
        Setup();
        _hubWorldManager.OnDoorOver += DoorOver;
        _hubWorldManager.OnDoorExit += DoorExit;
    }

    private void OnDisable()
    {
        _hubWorldManager.OnDoorOver -= DoorOver;
        _hubWorldManager.OnDoorExit -= DoorExit;
    }

    private void Setup()
    {
        _hubWorldManager = GetComponent<HubWorldManager>();
    }

    private void DoorOver(GameObject hubWorldDoor)
    {
        ShowAndHideInteractable doorInteractable = hubWorldDoor.GetComponent<ShowAndHideInteractable>();
        if (!_isDoorOver && doorInteractable != null)
        {
            doorInteractable.ShowInteractable();
            _isDoorOver = true;
        }
    }

    private void DoorExit(GameObject hubWorldDoor)
    {
        if (_isDoorOver)
        {
            hubWorldDoor.GetComponent<ShowAndHideInteractable>().HideInteractable();
            _isDoorOver = false;
        }
    }
}
