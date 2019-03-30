using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmationMenu : MonoBehaviour {

	private void Start()
	{
        gameObject.SetActive(false);
    }

	public void GoToMainMenu()
	{
        Time.timeScale = 1;
        LevelChanger.instance.FadeToLevel(1);
        gameObject.SetActive(true);
    }
}
