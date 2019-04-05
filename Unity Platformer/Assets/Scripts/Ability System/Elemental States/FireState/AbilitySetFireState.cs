using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySetFireState : MonoBehaviour
{
	private AbilityManager _abilityManager;

	private void OnEnable()
	{
		Setup();
		_abilityManager.OnFireState += SetFireState;
	}

	private void OnDisable()
	{
		_abilityManager.OnFireState -= SetFireState;
	}

	private void Setup()
	{
		_abilityManager = GetComponent<AbilityManager>();
	}

	private void SetFireState()
	{
		// Change the current player elemental state variable to fire state
		_abilityManager.CurrentPlayerElementalState = ElementalStates.Fire;
		// Set player's mass to its original value
        _abilityManager.playerRb.mass = _abilityManager.OriginalMass;
        Debug.Log(_abilityManager.CurrentPlayerElementalState);
	}
}
