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
		if (col.CompareTag("Player") && !_isNextLevel)
		{
            _isNextLevel = true;
            StartCoroutine(NextLevel(col.gameObject, _levelChangePE));
		}
	}

	private IEnumerator NextLevel(GameObject player, GameObject nextLevelPoofPE)
	{	
		yield return new WaitForSeconds(0.2f);
		GameObject nextLevelPoof = Instantiate(nextLevelPoofPE, player.transform.position, Quaternion.identity);
		player.SetActive(false);
		Destroy(nextLevelPoof, 1f);
		yield return new WaitForSeconds(0.5f);
        LevelChanger.instance.FadeToLevel(3);
        yield return new WaitForSeconds(1f);
        player.SetActive(true);
        _isNextLevel = false;
    }
}
