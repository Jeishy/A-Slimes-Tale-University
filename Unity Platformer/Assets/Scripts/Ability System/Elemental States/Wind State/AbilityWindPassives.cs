using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityWindPassives : MonoBehaviour {

	private AbilityManager abilityManager;
	private PlayerController playerController;
	[SerializeField] private float windGravityDecrease;
	[SerializeField] private float windVerticalJumpForceDecrease;
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
		playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}

	private void WindPassives()
	{
		playerController.gravity += windGravityDecrease;
		playerController.verticalJumpForce -= windVerticalJumpForceDecrease;
	}
}
