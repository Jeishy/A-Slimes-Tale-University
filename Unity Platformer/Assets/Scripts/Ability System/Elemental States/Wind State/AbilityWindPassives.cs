using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityWindPassives : MonoBehaviour {

	private AbilityManager abilityManager;
	private Rigidbody2D playerRB;
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
        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

	private void WindPassives()
	{
        playerRB.gravityScale -= windGravityDecrease;
	}
}
