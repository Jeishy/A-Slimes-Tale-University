using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySwtich : MonoBehaviour {
	private AbilityManager abilityManager;
	
	private void OnEnable()
	{
		Setup();
		abilityManager.OnPlayerSwitchAbility += Switch;
	}

	private void OnDisable()
	{
		abilityManager.OnPlayerSwitchAbility -= Switch;
	}

	private void Setup()
	{
		abilityManager = GetComponent<AbilityManager>();
	}

	private void Switch()
	{
		ElementalStates state = abilityManager.CurrentPlayerElementalState;
		// Can use switch
		// or
		// Can cycle through enum states like ints
		switch (state)
		{
			// Note: Consider case where player comes from state of None
			case ElementalStates.None:
				state = ElementalStates.Fire;
				break;
			case ElementalStates.Fire:
				state = ElementalStates.Water;
				break;
			case ElementalStates.Water:
				state = ElementalStates.Wind;
				break;
			case ElementalStates.Wind:
				state = ElementalStates.Earth;
				break;
			case ElementalStates.Earth:
				state = ElementalStates.Fire;
				break;
		}
		abilityManager.CurrentPlayerElementalState = state;
		Debug.Log(abilityManager.CurrentPlayerElementalState);
	}
}
