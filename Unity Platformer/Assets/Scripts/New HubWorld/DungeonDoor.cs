using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonDoor : MonoBehaviour {

    [SerializeField] private HubWorldManager _hubWorldManager;

	private void OnTriggerEnter(Collider col)
	{
        if (col.CompareTag("Player") && !_hubWorldManager.IsDoorInRange)
		{
            _hubWorldManager.IsDoorInRange = true;
            Debug.Log("Dungeon door in range");
            _hubWorldManager.DoorHoveredOver = transform.parent.gameObject;
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
