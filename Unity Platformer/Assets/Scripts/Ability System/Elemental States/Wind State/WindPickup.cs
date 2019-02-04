using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindPickup : MonoBehaviour {
	private AbilityManager abilityManager;
	// Use this for initialization
	void Start () {
		abilityManager = GameObject.Find("AbilityManager").GetComponent<AbilityManager>();
	}
	
	private void OnTriggerEnter(Collider col)
	{
		if (col.CompareTag("Player"))
		{
			abilityManager.WindState();
			Destroy(gameObject);
		}
	}
}
