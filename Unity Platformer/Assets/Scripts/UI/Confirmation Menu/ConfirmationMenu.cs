using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmationMenu : MonoBehaviour {

    [SerializeField] private HubWorldManager _hubWorldManager;

    [HideInInspector] public int BuildIndex;
    private void Start()
	{
        gameObject.SetActive(false);
    }

	public void GoToLevel()
	{
        Time.timeScale = 1;
        LevelChanger.instance.FadeToLevel(BuildIndex);
        gameObject.SetActive(true);
		_hubWorldManager.IsDoorSelected = false;
    }

	public void Continue()
	{
        Time.timeScale = 1;
        gameObject.SetActive(false);
        _hubWorldManager.IsDoorSelected = false;
    }
}
