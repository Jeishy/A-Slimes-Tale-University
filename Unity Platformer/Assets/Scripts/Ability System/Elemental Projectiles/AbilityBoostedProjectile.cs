using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBoostedProjectile : MonoBehaviour {

    [SerializeField] private Transform _projectileSpawnTrans;
    [SerializeField] private GameObject _boostedFireProj;
    [SerializeField] private GameObject _boostedWaterProj;
    [SerializeField] private GameObject _boostedEarthProj;
    [SerializeField] private GameObject _boostedWindProj;
    [Space]
    [SerializeField] private GameObject _fireMuzzleFlash;
    [SerializeField] private GameObject _waterMuzzleFlash;
    [SerializeField] private GameObject _windMuzzleFlash;
    [SerializeField] private GameObject _earthMuzzleFlash;

    private AbilityManager _abilityManager;
    private Player _playerDurability;
    private Transform _playerTrans;
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
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _playerTrans = player.GetComponent<Transform>();
        _playerDurability = player.GetComponent<Player>();
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
                    GameObject fire = Instantiate(_boostedFireProj, _projectileSpawnTrans.position, Quaternion.identity);
                    _boostedProjectile = fire;
                    fire.GetComponent<FireProjectile>().Shoot();
                    GameObject fireMf = Instantiate(_fireMuzzleFlash, _projectileSpawnTrans.position, Quaternion.identity, _playerTrans);
                    Destroy(fireMf, 1f);
                    break;
                case ElementalStates.Water:
                    GameObject water = Instantiate(_boostedWaterProj, _projectileSpawnTrans.position, Quaternion.identity);
                    _boostedProjectile = water;
                    water.GetComponent<WaterProjectile>().Shoot();
                    GameObject waterMf = Instantiate(_waterMuzzleFlash, _projectileSpawnTrans.position, Quaternion.identity);
                    Destroy(waterMf, 1f);
                    break;
                case ElementalStates.Wind:
                    GameObject wind = Instantiate(_boostedWindProj, _projectileSpawnTrans.position, Quaternion.identity);
                    _boostedProjectile = wind;
                    wind.GetComponent<WindProjectile>().Shoot();
                    GameObject windMf = Instantiate(_windMuzzleFlash, _projectileSpawnTrans.position, Quaternion.identity);
                    Destroy(windMf, 1f);
                    break;
                case ElementalStates.Earth:
                    GameObject earth = Instantiate(_boostedEarthProj, _projectileSpawnTrans.position, Quaternion.identity);
                    _boostedProjectile = earth;
                    earth.GetComponent<EarthProjectile>().Shoot();
                    GameObject earthMf = Instantiate(_earthMuzzleFlash, _projectileSpawnTrans.position, Quaternion.identity);
                    Destroy(earthMf, 1f);
                    break;
                case ElementalStates.None:
                    // Update UI or play particle effect here
                    //Debug.Log("State is None!");
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

	private static void SetupBoostedProjectile(GameObject projectile)
	{
		// Set projectile to be boosted
		projectile.GetComponent<ElementalProjectiles>().IsBoosted = true;
	}
}
