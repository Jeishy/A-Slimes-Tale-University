using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityEarthCrash : MonoBehaviour {
	private AbilityManager abilityManager;
	[SerializeField] private CharacterController charController;
	[SerializeField] private float downwardForce;
	[SerializeField] private float maxDamage;
	private bool isPlayerGravityChanged;
	private void OnEnable()
	{
		Setup();
		abilityManager.OnEarthCrash += EarthCrash;
	}

	private void OnDisable()
	{
		abilityManager.OnEarthCrash -= EarthCrash;		
	}

	private void Setup()
	{
		abilityManager = GetComponent<AbilityManager>();
		isPlayerGravityChanged = false;
	}

	private void EarthCrash()
	{
		//Vector3 downwardForceVector = new Vector3(0, -downwardForce, 0);
		//charController.Move(downwardForceVector);
	}
}
