using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityProjectile : MonoBehaviour {
    private AbilityManager abilityManager;
    private PlayerDurability playerDurability;
    [SerializeField] private Transform projectileSpawnTrans;
    [SerializeField] private ElementalProjectilePooler projPooler;
    
    public float fireRate;                                              // Used by AbilityInputHandler class to determine rate of fire
    

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
        playerDurability = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDurability>();
    }

    private void SpawnProjectile()
    {
        ElementalStates state = abilityManager.CurrentPlayerElementalState;
        // 0 = Fire, 1 = Water, 2 = Wind, 3 = Earth
        if (playerDurability.armour > 0)
        {
            playerDurability.armour--;
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
            if (abilityManager.CurrentPlayerElementalState != ElementalStates.None)
                abilityManager.CurrentPlayerElementalState = ElementalStates.None;
        }         
    }
}
