using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBoostedProjectile : MonoBehaviour {

    [SerializeField] private Transform projectileSpawnTrans;
    [SerializeField] private ElementalProjectilePooler projPooler;
	[SerializeField] private float projectileSizeIncrease;
	private AbilityManager abilityManager;
    private PlayerDurability playerDurability;

	private GameObject boostedProjectile;

	private void OnEnable()
	{
		Setup();
		abilityManager.OnBoostedProjectileFire += SpawnBoostedProjectile;
	}

	private void OnDisable()
	{		
		abilityManager.OnBoostedProjectileFire -= SpawnBoostedProjectile;
	}
	
	private void Setup()
	{
		abilityManager = GetComponent<AbilityManager>();
        playerDurability = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDurability>();
	}

	private void SpawnBoostedProjectile()
	{
		ElementalStates state = abilityManager.CurrentPlayerElementalState;

		if (playerDurability.armour > 0)
		{
			// Remove armour slot if boosted projectile is fired
			playerDurability.RemoveArmourSlot();

			switch (state)
            {
                case ElementalStates.Fire:
                    boostedProjectile = projPooler.SpawnProjectileFromPool("FireProj", projectileSpawnTrans.position, Quaternion.identity);
                    break;
                case ElementalStates.Water:
                    boostedProjectile = projPooler.SpawnProjectileFromPool("WaterProj", projectileSpawnTrans.position, Quaternion.identity);
                    break;
                case ElementalStates.Wind:
                    boostedProjectile = projPooler.SpawnProjectileFromPool("WindProj", projectileSpawnTrans.position, Quaternion.identity);
                    break;
                case ElementalStates.Earth:
                    boostedProjectile = projPooler.SpawnProjectileFromPool("EarthProj", projectileSpawnTrans.position, Quaternion.identity);
                    break;
                case ElementalStates.None:
                    // Update UI or play particle effect here
                    Debug.Log("State is None!");
                    break;
                default:
                    Debug.LogWarning("Ability state not set!");
                    break;
            }

			SetupBoostedProjectile(boostedProjectile);
		}
		else
		{
			// Update UI to screen
			// Set players current state to none
			if (abilityManager.CurrentPlayerElementalState != ElementalStates.None)
				abilityManager.CurrentPlayerElementalState = ElementalStates.None;
		}
	}

	private void SetupBoostedProjectile(GameObject projectile)
	{
		// Enlarge size of projectile
		projectile.transform.localScale += new Vector3(projectileSizeIncrease, projectileSizeIncrease, projectileSizeIncrease);

		// Set projectile to be boosted
		projectile.GetComponent<ElementalProjectiles>().IsBoosted = true;
	}
}
