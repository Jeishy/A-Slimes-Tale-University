using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySetStateWater : MonoBehaviour {

	private AbilityManager abilityManager;

	private void OnEnable()
	{
		Setup();
		abilityManager.OnWaterState += SetWaterState;
	}

	private void OnDisable()
	{
		abilityManager.OnWaterState -= SetWaterState;
	}

	private void Setup()
	{
		abilityManager = GetComponent<AbilityManager>();
    }

	private void SetWaterState()
	{
		abilityManager.CurrentPlayerElementalState = ElementalStates.Water;
        abilityManager.playerRB.gravityScale = abilityManager.InitialGravityScale;
        Debug.Log(abilityManager.CurrentPlayerElementalState);
	}
}
