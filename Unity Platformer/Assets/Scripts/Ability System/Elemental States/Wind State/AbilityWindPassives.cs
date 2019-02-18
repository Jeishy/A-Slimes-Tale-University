using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityWindPassives : MonoBehaviour {

	private AbilityManager abilityManager;
	[SerializeField] private Rigidbody2D rb;
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
	}

	private void WindPassives()
	{
		rb.gravityScale += windGravityDecrease;
	}
}
