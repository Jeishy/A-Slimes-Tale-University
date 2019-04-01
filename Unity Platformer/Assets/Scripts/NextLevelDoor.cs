using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelDoor : MonoBehaviour {

	[SerializeField] private GameObject _levelChangePE;
    private bool _isNextLevel;

	private void Start()
	{
        _isNextLevel = false;
    }

    private void OnTriggerEnter(Collider col)
	{
		if (col.CompareTag("Player") && !_isNextLevel && GameManager.instance.IsLevelComplete)
		{
            _isNextLevel = true;
            StartCoroutine(NextLevel(col.gameObject, _levelChangePE));
		}
	}

	private IEnumerator NextLevel(GameObject player, GameObject nextLevelPoofPE)
	{	
		// Wait to spawn particle effect
		yield return new WaitForSeconds(0.2f);
		// Spawn and destroy particle effect
		GameObject nextLevelPoof = Instantiate(nextLevelPoofPE, player.transform.position, Quaternion.identity);
		// Deactive player GO to give illusion that player has teleported
		player.SetActive(false);
		Destroy(nextLevelPoof, 1f);
		// Delay before going to the next level
		yield return new WaitForSeconds(0.5f);
        LevelChanger.instance.FadeToLevel(3);
		// Delay before reactivating player GO, so that player doesn't reappear during fade out animation
        yield return new WaitForSeconds(1f);
        player.SetActive(true);
		// Reset variables
        _isNextLevel = false;
        GameManager.instance.IsLevelComplete = false;
        GameManager.instance.gemstones = 0;
    }
}
