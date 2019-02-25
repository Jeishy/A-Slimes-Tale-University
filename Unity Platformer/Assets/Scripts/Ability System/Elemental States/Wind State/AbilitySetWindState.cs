using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySetWindState : MonoBehaviour {

	private AbilityManager abilityManager;
	private void OnEnable()
	{
		Setup();
		abilityManager.OnWindState += SetWindState;
	}

	private void OnDisable()
	{
		abilityManager.OnWindState -= SetWindState;
	}

	private void Setup()
	{
		abilityManager = GetComponent<AbilityManager>();
	}

	// Method for setting the elemental state of the player to Wind
	private void SetWindState()
	{
		abilityManager.CurrentPlayerElementalState = ElementalStates.Wind;
	}
}
