using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaFall : MonoBehaviour {
	[SerializeField] private float _knockbackMultiplier;
	[SerializeField] private Collider _solidCol;
	
	private AbilityManager _abilityManager;
	private Player _player;

	// Use this for initialization
	private void Start () 
	{
		_abilityManager = GameObject.FindGameObjectWithTag("AbilityManager").GetComponent<AbilityManager>();
		GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
		_player = playerGO.GetComponent<Player>();
	}

	private void OnTriggerStay(Collider col)
	{
		if (col.CompareTag("Player"))
		{
			ElementalStates state = _abilityManager.CurrentPlayerElementalState;
			Rigidbody rb = col.GetComponent<Rigidbody>();
			switch (state)
			{
				case ElementalStates.Water:
					// Play water evaporate particle effects
					Vector3 vel = rb.velocity;
					// Knocback player if entered the lavafall in water state 
					rb.velocity = new Vector3(-vel.x * _knockbackMultiplier, vel.y, 0f);
					_solidCol.isTrigger = false;
					// Do damage to player
					//_player.Hit(_abilityManager.CurrentPlayerElementalState, ElementalStates.Fire);
					break;
				case ElementalStates.Earth:
					_solidCol.isTrigger = false;
                    break;
                case ElementalStates.Wind:
					_solidCol.isTrigger = false;
					break;
				case ElementalStates.None:
					_solidCol.isTrigger = false;
                    // Do damage to player
					//_player.Hit(_abilityManager.CurrentPlayerElementalState, ElementalStates.Fire);
                    break;
				case ElementalStates.Fire:
					// Allow player to pass through lavalfall if in fire state
					_solidCol.isTrigger = true;
					break;
			}
		}
	}
}
