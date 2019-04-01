using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    [SerializeField] private GameObject[] _menus;

    private void Start()
	{
        _menus[1].SetActive(false);
    }

	public void StartGame()
	{
        LevelChanger.instance.OnLevelComplete();
    }

	public void GoToSettings()
	{
        _menus[0].SetActive(false);
        _menus[1].SetActive(true);
    }

	public void GoToMainMenu()
	{
		_menus[1].SetActive(false);
		_menus[0].SetActive(true);
	}

	public void GoToQuitGame()
	{
        LevelChanger.instance.FadeToLevel(1);
        Application.Quit();
    }
}
