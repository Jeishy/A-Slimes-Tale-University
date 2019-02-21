using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{


	public static LevelChanger singleton;
	
	[SerializeField] private Animator animator;
	private int levelToLoad;


	private void Start()
	{
		//Make sure only 1 level changer is instantiated
		if (singleton != null)
		{
			Debug.LogWarning("Two level changers detected, remove one from the current scene!");
			this.enabled = false;
			return;
		}
		
		singleton = this;
	}


	public void FadeToLevel(int levelIndex)
	{
		levelToLoad = levelIndex;
		
		//Sets trigger within the animator
		animator.SetTrigger("FadeOut");
	}

	//Called when the FadeOut animation has finished playing
	public void OnFadeComplete()
	{
		SceneManager.LoadScene(levelToLoad);
	}
	
	
}
