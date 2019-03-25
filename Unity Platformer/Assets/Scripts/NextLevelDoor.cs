using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelDoor : MonoBehaviour {

	[SerializeField] private GameObject _levelChangePE;

	private LevelChanger _levelChanger;

	private void Start()
	{
		_levelChanger = GameObject.Find("_levelChanger").GetComponent<LevelChanger>();
	}

	private void OnTriggerEnter(Collider col)
	{
		if (col.CompareTag("Player"))
		{
			StartCoroutine(NextLevel(col.gameObject, _levelChangePE));
		}
	}

	private IEnumerator NextLevel(GameObject player, GameObject particleEffect)
	{	
		yield return new WaitForSeconds(0.2f);
		GameObject pe = Instantiate(particleEffect, player.transform.position, Quaternion.identity);
		player.SetActive(false);
		Destroy(pe, 1f);
		yield return new WaitForSeconds(0.5f);
		_levelChanger.OnLevelComplete();
		player.SetActive(true);
	}
}
