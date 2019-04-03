using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HubWorldInputHandler : MonoBehaviour {

    [SerializeField] private Animator _interactCanvasAnim;

    private HubWorldManager _hubWorldManager;
    private GameObject _door;
    private bool _isOpen;

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
		if (_hubWorldManager.IsDoorInRange && !_isOpen)
		{
            _isOpen = true;
            _interactCanvasAnim.SetTrigger("Open");
            _door = _hubWorldManager.DoorHoveredOver;
            _hubWorldManager.DoorOver(_door);
            // Run on hover over event

		}
        else if ( !_hubWorldManager.IsDoorInRange && _isOpen)
        {
            _isOpen = false;  
            _interactCanvasAnim.SetTrigger("Close");
            GameObject door = _hubWorldManager.DoorHoveredOver;
            _hubWorldManager.DoorExit(door);
        }

        if (Input.GetKeyDown(KeyCode.E) && _hubWorldManager.IsDoorInRange)
        {
            Debug.Log("Selecting a door");
            // Play particle effect, run animation, whatever
            _hubWorldManager.DoorSelected(_door);
        }
    }
}
