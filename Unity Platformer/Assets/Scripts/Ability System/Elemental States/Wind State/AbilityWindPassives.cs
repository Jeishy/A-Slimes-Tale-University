using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityWindPassives : MonoBehaviour {

	private AbilityManager _abilityManager;
	[SerializeField][Range(0.01f, 0.6f)] private float _windGravityDecrease;
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
        _abilityManager.playerRb.mass -= _windGravityDecrease;
	}
}
