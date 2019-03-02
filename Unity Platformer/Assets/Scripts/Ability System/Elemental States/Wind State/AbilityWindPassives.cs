using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityWindPassives : MonoBehaviour {

	private AbilityManager _abilityManager;
	[SerializeField] private float _windGravityDecrease;
	private void OnEnable()
	{
		Setup();
		_abilityManager.OnWindState += WindPassives;
	}

	private void OnDisable()
	{
		_abilityManager.OnWindState -= WindPassives;
	}

	private void Setup()
	{
		_abilityManager = GetComponent<AbilityManager>();
    }

	private void WindPassives()
	{
        _abilityManager.playerRb.gravityScale -= _windGravityDecrease;
	}
}
