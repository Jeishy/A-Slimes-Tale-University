using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDoor : MonoBehaviour 
{
	[SerializeField] private HubWorldManager _hubWorldManager;

	private void OnTriggerEnter(Collider col)
	{
		if (col.CompareTag("Player") && !_hubWorldManager.IsDoorInRange)
		{
			_hubWorldManager.IsDoorInRange = true;
			_hubWorldManager.DoorHoveredOver = gameObject;
		}
	}

	private void OnTriggerExit(Collider col)
	{
		if (col.CompareTag("Player") && _hubWorldManager.IsDoorInRange)
		{
			_hubWorldManager.IsDoorInRange = false;
		}
	}
}
