using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySetStateWater : MonoBehaviour {

	private AbilityManager _abilityManager;

	private void OnEnable()
	{
		Setup();
		_abilityManager.OnWaterState += SetWaterState;
	}

	private void OnDisable()
	{
		_abilityManager.OnWaterState -= SetWaterState;
	}

	private void Setup()
	{
		_abilityManager = GetComponent<AbilityManager>();
    }

	private void SetWaterState()
	{
		_abilityManager.CurrentPlayerElementalState = ElementalStates.Water;
        _abilityManager.playerRb.gravityScale = _abilityManager.InitialGravityScale;
        Debug.Log(_abilityManager.CurrentPlayerElementalState);
	}
}
