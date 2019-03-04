using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthPickup : MonoBehaviour {

	private AbilityManager _abilityManager;
        private Player player;
	void Start () {
		_abilityManager = GameObject.FindGameObjectWithTag("AbilityManager").GetComponent<AbilityManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
	
	// If wind pickup interacts with player,
	// set players elemental state to Wind
	// and run method for running OnWindState event
	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Player"))
		{
            player.AddArmourSlot();
            _abilityManager.EarthState();
			Destroy(gameObject);
		}
	}
}
