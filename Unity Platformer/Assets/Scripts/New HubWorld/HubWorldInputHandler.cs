using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HubWorldInputHandler : MonoBehaviour {

    [SerializeField] private Animator _interactCanvasAnim;

    private HubWorldManager _hubWorldManager;
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
            GameObject door = _hubWorldManager.DoorHoveredOver;
            _hubWorldManager.DoorOver(door);
            // Run on hover over event
            if (Input.GetKeyDown(KeyCode.E))
			{
                // Play particle effect, run animation, whatever
                _hubWorldManager.DoorSelected(door);
            }
		}
        else if ( !_hubWorldManager.IsDoorInRange && _isOpen)
        {
            _isOpen = false;  
            _interactCanvasAnim.SetTrigger("Close");
            GameObject door = _hubWorldManager.DoorHoveredOver;
            _hubWorldManager.DoorExit(door);
        }
	}
}
