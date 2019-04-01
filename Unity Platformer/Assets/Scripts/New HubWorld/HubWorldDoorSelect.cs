using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HubWorldDoorSelect : MonoBehaviour {

    [SerializeField] private GameObject _confirmationCanvas;
    [SerializeField] private TextMeshProUGUI _levelNameTxt;

    private HubWorldManager _hubWorldManager;
    private ConfirmationMenu _confirmMenu;
    private GameObject _player;

    private void OnEnable()
	{
		Setup();
        _hubWorldManager.OnDoorSelected += DoorSelect;
    }

	private void OnDisable()
	{
		_hubWorldManager.OnDoorSelected -= DoorSelect;
	}

    private void Setup()
	{
        _hubWorldManager = GetComponent<HubWorldManager>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _confirmMenu = _confirmationCanvas.GetComponent<ConfirmationMenu>();
    }

	private void DoorSelect(GameObject door)
	{
        if (!_hubWorldManager.IsDoorSelected)
		{
            // Display confirmation canvas
            _confirmationCanvas.SetActive(true);
			// Pause game
            Time.timeScale = 0;
            _hubWorldManager.IsDoorSelected = true; 

			string doorName = door.name;
            switch (doorName)
			{
				case "Exit Door":
					//_confirmMenu.BuildIndex = Array.Find;
                    _levelNameTxt.text = "";
                    break;
				case "Dungeon Door":
                    _confirmMenu.BuildIndex = 4;
                    _levelNameTxt.text = "Dungeon";
                    break;
				case "Wind Door":
					_confirmMenu.BuildIndex = 5;
                    _levelNameTxt.text = "China Mountain";
                    break;
			}
        }
    }
}
