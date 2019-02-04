using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityInputHandler : MonoBehaviour {

	private AbilityManager abilityManager;
	private AbilityProjectile abilityProjectile;
	private float projFireTime;
	private float projFireRate;

	// Use this for initialization
	void Start () {
		abilityManager = GetComponent<AbilityManager>();
		abilityProjectile = GetComponent<AbilityProjectile>();
		projFireTime = 0f;
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

	}

    private void ShootToggle()
    {
        abilityManager.IsAimToShoot = !abilityManager.IsAimToShoot;
    }

}
