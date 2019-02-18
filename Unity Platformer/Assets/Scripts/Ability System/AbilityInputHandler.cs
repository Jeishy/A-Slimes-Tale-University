using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityInputHandler : MonoBehaviour {

	private AbilityManager abilityManager;
	private AbilityProjectile abilityProjectile;
	// The projectile time, used to determine cooldown
	// of projectile
	private float projFireTime;
	// The fire rate of the projectile (seconds)
	private float projFireRate;

	// Use this for initialization
	void Start () {
		abilityManager = GetComponent<AbilityManager>();
		abilityProjectile = GetComponent<AbilityProjectile>();
		projFireTime = 0f;	// Set fire time to zero at beginning of level, Note: This must be set to 0 when each level is left/complete
	}
	
	// Update is called once per frame
	void Update () {
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
        if (Input.GetMouseButton(0))
        {
            if (Time.time > projFireTime)
            {
                // Set fire time variable to the fire rate + current time elapsed
                // ensures projectile only fired when new fire time is elapsed
				projFireRate = abilityProjectile.fireRate;
                projFireTime = projFireRate + Time.time;
                abilityManager.ProjectileFire();
            }
        }

		// Left control activates the earth element ability smash
		if (Input.GetKeyDown(KeyCode.LeftControl))
		{
			abilityManager.EarthCrash();
		}
	}

    private void ShootToggle()
    {
        abilityManager.IsAimToShoot = !abilityManager.IsAimToShoot;
    }

}
