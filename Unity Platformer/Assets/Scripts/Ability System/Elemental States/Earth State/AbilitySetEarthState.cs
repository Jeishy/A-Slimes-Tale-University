using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySetEarthState : MonoBehaviour {

	private AbilityManager abilityManager;
	private void OnEnable()
	{
		Setup();
		abilityManager.OnEarthState += SetEarthState;
	}

	private void OnDisable()
	{
		abilityManager.OnEarthState -= SetEarthState;
	}

	private void Setup()
	{
		abilityManager = GetComponent<AbilityManager>();
	}

	private void SetEarthState()
	{
		abilityManager.CurrentPlayerElementalState = ElementalStates.Earth;
		Debug.Log(abilityManager.CurrentPlayerElementalState);
	}
}
