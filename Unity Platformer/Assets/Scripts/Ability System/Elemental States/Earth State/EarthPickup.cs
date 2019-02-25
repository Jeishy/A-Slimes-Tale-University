using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthPickup : MonoBehaviour {

	private AbilityManager abilityManager;
	void Start () {
		abilityManager = GameObject.FindGameObjectWithTag("AbilityManager").GetComponent<AbilityManager>();
	}
	
	// If wind pickup interacts with player,
	// set players elemental state to Wind
	// and run method for running OnWindState event
	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Player"))
		{
			abilityManager.EarthState();
			Destroy(gameObject);
		}
	}
}
