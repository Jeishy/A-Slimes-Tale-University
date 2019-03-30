using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubWorldManager : MonoBehaviour {

#region Delegates and Events
	public delegate void HubWorldEventHandler(GameObject hubWorldDoor);
    public event HubWorldEventHandler OnDoorSelected;
    public event HubWorldEventHandler OnDoorOver;
    public event HubWorldEventHandler OnDoorExit;
    #endregion

    [HideInInspector] public bool IsDoorSelected;
    [HideInInspector] public bool IsDoorInRange;
    [HideInInspector] public GameObject DoorHoveredOver;

    private void Start()
	{
        IsDoorInRange = false;
        IsDoorSelected = false;
    }

    // Method for running methods subscribed to OnDoorSelected event
    public void DoorSelected(GameObject hubWorldDoor)
	{
		if (OnDoorSelected != null)
            OnDoorSelected(hubWorldDoor);
    }
}
