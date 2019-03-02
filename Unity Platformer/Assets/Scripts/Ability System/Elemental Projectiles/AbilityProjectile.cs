using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityProjectile : MonoBehaviour {
    private AbilityManager _abilityManager;
    private PlayerDurability _playerDurability;
    [SerializeField] private Transform _projectileSpawnTrans;
    [SerializeField] private ElementalProjectilePooler _projPooler;
    [SerializeField] private GameObject _fireMuzzleFlash;
    [SerializeField] private GameObject _waterMuzzleFlash;


    public float fireRate;                                              // Used by AbilityInputHandler class to determine rate of fire
    

    private void OnEnable()
    {
        Setup();
        _abilityManager.OnProjectileFire += SpawnProjectile;
    }

    private void OnDisable()
    {
        _abilityManager.OnProjectileFire -= SpawnProjectile;
    }

    private void Setup()
    {
        _abilityManager = GetComponent<AbilityManager>();
        _playerDurability = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDurability>();
    }

    private void SpawnProjectile()
    {
        ElementalStates state = _abilityManager.CurrentPlayerElementalState;
        // 0 = Fire, 1 = Water, 2 = Wind, 3 = Earth
        if (_playerDurability.armour > 0)
        {
            _playerDurability.armour--;
            switch (state)
            {
                case ElementalStates.Fire:
                    _projPooler.SpawnProjectileFromPool("FireProj", _projectileSpawnTrans.position, Quaternion.identity);
                    GameObject muzzleFlash = Instantiate(_fireMuzzleFlash, _projectileSpawnTrans.position, Quaternion.identity);
                    Destroy(muzzleFlash, 1f);
                    break;
                case ElementalStates.Water:
                    _projPooler.SpawnProjectileFromPool("WaterProj", _projectileSpawnTrans.position, Quaternion.identity);
                    GameObject muzzle = Instantiate(_waterMuzzleFlash, _projectileSpawnTrans.position, Quaternion.identity);
                    Destroy(muzzle, 1f);
                    break;
                case ElementalStates.Wind:
                    _projPooler.SpawnProjectileFromPool("WindProj", _projectileSpawnTrans.position, Quaternion.identity);
                    break;
                case ElementalStates.Earth:
                    _projPooler.SpawnProjectileFromPool("EarthProj", _projectileSpawnTrans.position, Quaternion.identity);
                    break;
                case ElementalStates.None:
                    // Update UI or play particle effect here
                    //Debug.Log("State is None!");
                    break;
                default:
                    Debug.LogWarning("Ability state not set!");
                    break;
            }
        }
        else
        {
            // Update UI to screen
            // Set players current state to None
            // Check if players state isnt already none
            Debug.Log("Used up all armour points! Elemental state set to none");
            if (_abilityManager.CurrentPlayerElementalState != ElementalStates.None)
                _abilityManager.CurrentPlayerElementalState = ElementalStates.None;
        }         
    }
}
