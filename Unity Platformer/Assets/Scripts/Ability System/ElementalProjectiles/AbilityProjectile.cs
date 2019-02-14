using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityProjectile : MonoBehaviour {
    private AbilityManager abilityManager;
    [SerializeField] private Transform projectileSpawnTrans;
    public float fireRate;
    [SerializeField] private ElementalProjectilePooler projPooler;

    private void OnEnable()
    {
        Setup();
        abilityManager.OnProjectileFire += SpawnProjectile;
    }

    private void OnDisable()
    {
        abilityManager.OnProjectileFire -= SpawnProjectile;
    }

    private void Setup()
    {
        abilityManager = GetComponent<AbilityManager>();
    }

    private void SpawnProjectile()
    {
        ElementalStates state = abilityManager.CurrentPlayerElementalState;
        // 0 = Fire, 1 = Water, 2 = Wind, 3 = Earth
        // Note: See enums list and AbilitySwitch script to see switch order          
        switch (state)
        {
            case ElementalStates.Fire:
                projPooler.SpawnProjectileFromPool("FireProj", projectileSpawnTrans.position, Quaternion.identity);
                break;
            case ElementalStates.Water:
                projPooler.SpawnProjectileFromPool("WaterProj", projectileSpawnTrans.position, Quaternion.identity);
                break;
            case ElementalStates.Wind:
                projPooler.SpawnProjectileFromPool("WindProj", projectileSpawnTrans.position, Quaternion.identity);
                break;
            case ElementalStates.Earth:
                projPooler.SpawnProjectileFromPool("EarthProj", projectileSpawnTrans.position, Quaternion.identity);
                break;
            case ElementalStates.None:
                Debug.Log("State is None!");
                break;
            default:
                Debug.LogWarning("Ability state not set!");
                break;
        }
    }
}
