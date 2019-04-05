using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AbilityInputHandler : MonoBehaviour {

	[SerializeField] private LayerMask _enemyLayerMask;
	[SerializeField] private float _boostedProjectileMaxTime;	// The longest time mouse button 0 must be held to spawn boosted projectile

    private AbilityManager _abilityManager;
	private AbilityProjectile _abilityProjectile;
	private AbilityEarthCrash _abilityEarthCrash;
	// The projectile time, used to determine cooldown
	// of projectile
	private float _projFireTime;
	// The fire rate of the projectile (seconds)
	private float _projFireRate;
	private CharacterController2D _characterController;
	private bool _isMouseZeroPressed; 
	private float _mousePressedStartTime;
	private float _mousePressedEndTime;


    // Use this for initialization
    private void Start () {
		_abilityManager = GetComponent<AbilityManager>();
		_abilityProjectile = GetComponent<AbilityProjectile>();
		_projFireTime = 0f;	// Set fire time to zero at beginning of level, Note: This must be set to 0 when each level is left/complete
        _characterController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
		_abilityEarthCrash = GetComponent<AbilityEarthCrash>();
        _isMouseZeroPressed = false;
    }
	
	// Update is called once per frame
	private void Update () {
		InputHandler();
		//EarthCrashCheck();
	}

    private void ShootToggle()
    {
        _abilityManager.IsAimToShoot = !_abilityManager.IsAimToShoot;
    }

	private void InputHandler()
	{
        // Toggles between ability states
        // Used for debugging
        if (Input.GetKeyDown(KeyCode.Q))
		{
			_abilityManager.PlayerSwitchAbility();
		}

        // Check if mouse button 0 (Left click) is clicked and 
        // if elapsed time is greater than fire time (Used for cooldown)
        if (Input.GetButtonDown("Fire1"))
        {
            if (!_isMouseZeroPressed)
			{
				_mousePressedStartTime = Time.time;
				_isMouseZeroPressed = true;
        	}
		}
		else if (Input.GetButtonUp("Fire1"))
		{
            _mousePressedEndTime = Time.time;
			float mousePressedDeltaTime = _mousePressedEndTime - _mousePressedStartTime;
            //Debug.Log(mousePressedDeltaTime);
			_isMouseZeroPressed = false;

			if (Time.time > _projFireTime && mousePressedDeltaTime < _boostedProjectileMaxTime)
			{
				// Set fire time variable to the fire rate + current time elapsed
				// ensures projectile only fired when new fire time is elapsed
				_projFireRate = _abilityProjectile.fireRate;
				_projFireTime = _projFireRate + Time.time;
                _abilityManager.ProjectileFire();
            }
            /*else if (mousePressedDeltaTime > _boostedProjectileMaxTime)
			{
				// Spawn boosted projectile if mouse 0 is pressed long enough
				_projFireRate = _abilityProjectile.fireRate;
				_projFireTime = _projFireRate + Time.time;
                _abilityManager.BoostedProjectileFire();
            }*/
        }

		// Left control activates the earth element ability crash
		if (Input.GetKeyDown(KeyCode.LeftControl) && _abilityManager.CurrentPlayerElementalState == ElementalStates.Earth && !_characterController.m_Grounded)
		{
			_abilityManager.EarthCrash();
		}
	}

	private void EarthCrashCheck()
	{
        if (!_characterController.m_Grounded || !_abilityEarthCrash.IsCrashAbilityActivated) return;

        // Get all enemy coliiders in range
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(_characterController.transform.position, _abilityEarthCrash.SplashRadius, _enemyLayerMask);
        if (enemyColliders.Length > 0)
        {
            // If there are enemies in range, do splash damage
            _abilityEarthCrash.SplashDamage(enemyColliders);
        }
        _abilityEarthCrash.IsCrashAbilityActivated = false;
    }
}
