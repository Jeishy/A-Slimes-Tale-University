using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySetWindState : MonoBehaviour {

	private AbilityManager _abilityManager;
	private void OnEnable()
	{
		Setup();
		_abilityManager.OnWindState += SetWindState;
	}

	private void OnDisable()
	{
		_abilityManager.OnWindState -= SetWindState;
	}

	private void Setup()
	{
		_abilityManager = GetComponent<AbilityManager>();
	}

	// Method for setting the elemental state of the player to Wind
	private void SetWindState()
	{
		_abilityManager.CurrentPlayerElementalState = ElementalStates.Wind;
	}
}
