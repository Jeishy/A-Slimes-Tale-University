using UnityEngine;

public class UserInterface : MonoBehaviour
{

	//Pause Menu
	[SerializeField] private CanvasGroup pauseMenu;
	[SerializeField] private CanvasGroup settingsMenu;
	[SerializeField] private bool timeStopAtPause;

	private bool pauseMenuShowing;
	private bool settingsMenuShowing;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Pause"))
		{


			pauseMenuShowing = !pauseMenuShowing;

			if (pauseMenuShowing)
			{
				Time.timeScale = 0f;
				pauseMenu.alpha = 1f;
				pauseMenu.blocksRaycasts = false;
			} else
            {
                OnContinueGame();
            }
		}
	}

	public void OnContinueGame()
	{

        Debug.Log("Hide pause");

		Time.timeScale = 1f;
		pauseMenuShowing = false;
		pauseMenu.alpha = 0f;
		pauseMenu.blocksRaycasts = false;


        settingsMenuShowing = false;
        settingsMenu.alpha = 0f;
        settingsMenu.blocksRaycasts = false;

    }

	public void OnSettingsOpen()
	{
		//Hide pause menu and show settings menu
		pauseMenu.alpha = 0f;
		pauseMenu.blocksRaycasts = false;
		
		settingsMenuShowing = true;
		settingsMenu.alpha = 1f;
		settingsMenu.blocksRaycasts = true;
		
		
	}

	public void OnMusicToggle(bool toggle)
	{
		Debug.Log("Music: " + toggle);
	}
	
	
	public void OnSettingsClose()
	{
		//Show pause menu and hide settings menu
		pauseMenu.alpha = 1f;
		pauseMenu.blocksRaycasts = true;
		
		settingsMenuShowing = false;
		settingsMenu.alpha = 0f;
		settingsMenu.blocksRaycasts = false;
	}

	public void OnMasterVolumeChange(float volume)
	{
		AudioListener.volume = volume;
	}
	
	public void OnMusicVolumeChange(float volume)
	{
		Debug.Log("Music volume changed: " + volume + "/1");
	}
	
    public void OnQuitToHubWorld()
    {
        OnContinueGame();
        LevelChanger.instance.FadeToLevel(1);
    }

	public void OnQuitGame()
	{
		Application.Quit();
	}
}
