using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubWorldDoorSelect : MonoBehaviour {

    [SerializeField] private GameObject[] portalSelectPEs = new GameObject[3];
    [SerializeField] private GameObject _ConfirmationCanvas;

    private HubWorldManager _hubWorldManager;
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
    }

	private void DoorSelect(GameObject door)
	{
        if (!_hubWorldManager.IsDoorSelected)
		{
			_hubWorldManager.IsDoorSelected = true;
			string doorName = door.name;
            switch (doorName)
			{
				case "Exit Door":
                    // Display confirmation canvas
                    _ConfirmationCanvas.SetActive(true);
                    Time.timeScale = 0;
                    break;
				case "Fire Door":
					StartCoroutine(GoToLevel(3));
					break;
				case "Wind Door":
                    StartCoroutine(GoToLevel(4));
                    break;
			}
		}
    }

	private IEnumerator GoToLevel(int buildIndex/*, GameObject portalSelectPE*/)
	{
		//yield return new WaitForSeconds(0.2f);
		//GameObject pe = Instantiate(portalSelectPE, _player.transform.position, Quaternion.identity);
		//_player.SetActive(false);
		//Destroy(pe, 1f);
		yield return new WaitForSeconds(0.5f);
        LevelChanger.instance.FadeToLevel(buildIndex);
        //_player.SetActive(true);
    }
}
