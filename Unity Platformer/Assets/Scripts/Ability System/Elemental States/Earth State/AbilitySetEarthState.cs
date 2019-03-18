using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySetEarthState : MonoBehaviour {

	private AbilityManager _abilityManager;
	private void OnEnable()
	{
		Setup();
		_abilityManager.OnEarthState += SetEarthState;
	}

	private void OnDisable()
	{
		_abilityManager.OnEarthState -= SetEarthState;
	}

	private void Setup()
	{
		_abilityManager = GetComponent<AbilityManager>();
	}

	private void SetEarthState()
	{
		_abilityManager.CurrentPlayerElementalState = ElementalStates.Earth;
        _abilityManager.playerRb.mass = _abilityManager.OriginalMass;
		Debug.Log(_abilityManager.CurrentPlayerElementalState);
	}
}
