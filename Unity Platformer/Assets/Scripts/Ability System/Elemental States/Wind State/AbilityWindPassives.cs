using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityWindPassives : MonoBehaviour {

	private AbilityManager abilityManager;
	[SerializeField] private float windGravityDecrease;
	private void OnEnable()
	{
		Setup();
		abilityManager.OnWindState += WindPassives;
	}

	private void OnDisable()
	{
		abilityManager.OnWindState -= WindPassives;
	}

	private void Setup()
	{
		abilityManager = GetComponent<AbilityManager>();
    }

	private void WindPassives()
	{
        abilityManager.playerRB.gravityScale -= windGravityDecrease;
	}
}
