﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubWorldInputHandler : MonoBehaviour {

    private HubWorldManager _hubWorldManager;

	private void Start()
	{
        _hubWorldManager = GetComponent<HubWorldManager>();
    }
    // Update is called once per frame
    void Update () {
		DoorInteract();
    }

	private void DoorInteract()
	{
		if (_hubWorldManager.IsDoorInRange)
		{
			GameObject door = _hubWorldManager.DoorHoveredOver;
			// Run on hover over event
			if (Input.GetKeyDown(KeyCode.E))
			{
                // Play particle effect, run animation, whatever
                _hubWorldManager.DoorSelected(door);
            }
		}
	}
}
