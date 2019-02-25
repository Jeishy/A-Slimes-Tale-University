using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySetFireState : MonoBehaviour {

	private AbilityManager abilityManager;
	private void OnEnable()
	{
		Setup();
		abilityManager.OnFireState += SetFireState;
	}

	private void OnDisable()
	{
		abilityManager.OnFireState -= SetFireState;
	}

	private void Setup()
	{
		abilityManager = GetComponent<AbilityManager>();
	}

	private void SetFireState()
	{
		abilityManager.CurrentPlayerElementalState = ElementalStates.Fire;
        abilityManager.playerRB.gravityScale = abilityManager.InitialGravityScale;
        Debug.Log(abilityManager.CurrentPlayerElementalState);
	}
}
