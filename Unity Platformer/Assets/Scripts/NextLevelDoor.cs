﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelDoor : MonoBehaviour {

	[SerializeField] private GameObject _levelChangePE;

	private void OnTriggerEnter(Collider col)
	{
		if (col.CompareTag("Player"))
		{
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
		LevelChanger.instance.OnLevelComplete();
		player.SetActive(true);
	}
}
