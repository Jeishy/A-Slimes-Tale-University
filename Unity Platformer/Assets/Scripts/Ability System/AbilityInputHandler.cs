using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityInputHandler : MonoBehaviour {

	[SerializeField] private LayerMask enemyLayerMask;
	[SerializeField] private float boostedProjectileMaxTime;	// The longest time mouse button 0 must be held to spawn boosted projectile
	private AbilityManager abilityManager;
	private AbilityProjectile abilityProjectile;
	private AbilityEarthCrash abilityEarthCrash;
	// The projectile time, used to determine cooldown
	// of projectile
	private float projFireTime;
	// The fire rate of the projectile (seconds)
	private float projFireRate;
	private CharacterController2D characterController;
	private bool isMouseZeroPressed; 
	private float mousePressedStartTime;
	private float mousePressedEndTime;

	// Use this for initialization
	void Start () {
		abilityManager = GetComponent<AbilityManager>();
		abilityProjectile = GetComponent<AbilityProjectile>();
		projFireTime = 0f;	// Set fire time to zero at beginning of level, Note: This must be set to 0 when each level is left/complete
		characterController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
		abilityEarthCrash = GetComponent<AbilityEarthCrash>();
	}
	
	// Update is called once per frame
	void Update () {
		InputHandler();
		EarthCrashCheck();
	}

    private void ShootToggle()
    {
        abilityManager.IsAimToShoot = !abilityManager.IsAimToShoot;
    }

	private void InputHandler()
	{
		// Toggles aiming mode from aimed to forward
        // Note: Ensure button mapping is set for this action
        // instead of Input.GetKeyDown
        if (Input.GetKeyDown(KeyCode.E))
        {
			ShootToggle();
        }

		// Toggles between ability states
		// Used for debugging
		if (Input.GetKeyDown(KeyCode.Q))
		{
			abilityManager.PlayerSwitchAbility();
		}

		// Check if mouse button 0 (Left click) is clicked and 
        // if elapsed time is greater than fire time (Used for cooldown)
        if (Input.GetMouseButtonDown(0))
        {
			if (!isMouseZeroPressed)
			{
				mousePressedStartTime = Time.time;
				isMouseZeroPressed = true;
        	}
		}
		else if (Input.GetMouseButtonUp(0))
		{
			mousePressedEndTime = Time.time;
			float mousePressedDeltaTime = mousePressedEndTime - mousePressedStartTime;
			isMouseZeroPressed = false;

			if (Time.time > projFireTime && mousePressedDeltaTime < boostedProjectileMaxTime)
			{
				// Set fire time variable to the fire rate + current time elapsed
				// ensures projectile only fired when new fire time is elapsed
				projFireRate = abilityProjectile.fireRate;
				projFireTime = projFireRate + Time.time;
				abilityManager.ProjectileFire();
			}
			else if (mousePressedDeltaTime > boostedProjectileMaxTime)
			{
				// Spawn boosted projectile if mouse 0 is pressed long enough
				projFireRate = abilityProjectile.fireRate;
				projFireTime = projFireRate + Time.time;
				abilityManager.BoostedProjectileFire();
			}
		}

		// Left control activates the earth element ability crash
		if (Input.GetKeyDown(KeyCode.LeftControl) && abilityManager.CurrentPlayerElementalState == ElementalStates.Earth && !characterController.m_Grounded)
		{
			abilityManager.EarthCrash();
		}

	}

	private void EarthCrashCheck()
	{
		if (characterController.m_Grounded && abilityEarthCrash.IsCrashAbilityActivated)
		{
			// Get all enemy coliiders in range
			Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(characterController.transform.position, abilityEarthCrash.SplashRadius, enemyLayerMask);
			if (enemyColliders.Length > 0)
			{
				// If there are enemies in range, do splash damage
				abilityEarthCrash.SplashDamage(enemyColliders);
			}
			abilityEarthCrash.IsCrashAbilityActivated = false;
		}
	}
}
