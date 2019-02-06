using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindPickup : MonoBehaviour {

	private AbilityManager abilityManager;
	void Start () {
		abilityManager = GameObject.FindGameObjectWithTag("AbilityManager").GetComponent<AbilityManager>();
	}
	
	// If wind pickup interacts with player,
	// set players elemental state to Wind
	// and run method for running OnWindState event
	private void OnTriggerEnter(Collider col)
	{
		if (col.CompareTag("Player"))
		{
			abilityManager.WindState();
			Destroy(gameObject);
		}
	}
}
