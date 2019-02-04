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

	private void SetWindState()
	{
		abilityManager.CurrentPlayerElementalState = ElementalStates.Wind;
		Debug.Log(abilityManager.CurrentPlayerElementalState);
	}
}
