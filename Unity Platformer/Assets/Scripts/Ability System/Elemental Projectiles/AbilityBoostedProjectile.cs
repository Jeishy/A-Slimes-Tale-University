using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBoostedProjectile : MonoBehaviour {

    [SerializeField] private Transform _projectileSpawnTrans;
    [SerializeField] private ElementalProjectilePooler _projPooler;
	[SerializeField] private float _projectileSizeIncrease;
	private AbilityManager _abilityManager;
    private PlayerDurability _playerDurability;

	private GameObject _boostedProjectile;

	private void OnEnable()
	{
		Setup();
		_abilityManager.OnBoostedProjectileFire += SpawnBoostedProjectile;
	}

	private void OnDisable()
	{		
		_abilityManager.OnBoostedProjectileFire -= SpawnBoostedProjectile;
	}
	
	private void Setup()
	{
		_abilityManager = GetComponent<AbilityManager>();
        _playerDurability = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDurability>();
	}

	private void SpawnBoostedProjectile()
	{
		ElementalStates state = _abilityManager.CurrentPlayerElementalState;

		if (_playerDurability.armour > 0)
		{
			// Remove armour slot if boosted projectile is fired
			_playerDurability.RemoveArmourSlot();

			switch (state)
            {
                case ElementalStates.Fire:
                    _boostedProjectile = _projPooler.SpawnProjectileFromPool("FireProj", _projectileSpawnTrans.position, Quaternion.identity);
                    break;
                case ElementalStates.Water:
                    _boostedProjectile = _projPooler.SpawnProjectileFromPool("WaterProj", _projectileSpawnTrans.position, Quaternion.identity);
                    break;
                case ElementalStates.Wind:
                    _boostedProjectile = _projPooler.SpawnProjectileFromPool("WindProj", _projectileSpawnTrans.position, Quaternion.identity);
                    break;
                case ElementalStates.Earth:
                    _boostedProjectile = _projPooler.SpawnProjectileFromPool("EarthProj", _projectileSpawnTrans.position, Quaternion.identity);
                    break;
                case ElementalStates.None:
                    // Update UI or play particle effect here
                    Debug.Log("State is None!");
                    break;
                default:
                    Debug.LogWarning("Ability state not set!");
                    break;
            }

			SetupBoostedProjectile(_boostedProjectile);
		}
		else
		{
			// Update UI to screen
			// Set players current state to none
			if (_abilityManager.CurrentPlayerElementalState != ElementalStates.None)
				_abilityManager.CurrentPlayerElementalState = ElementalStates.None;
		}
	}

	private void SetupBoostedProjectile(GameObject projectile)
	{
		// Enlarge size of projectile
		projectile.transform.localScale += new Vector3(_projectileSizeIncrease, _projectileSizeIncrease, _projectileSizeIncrease);

		// Set projectile to be boosted
		projectile.GetComponent<ElementalProjectiles>().IsBoosted = true;
	}
}
