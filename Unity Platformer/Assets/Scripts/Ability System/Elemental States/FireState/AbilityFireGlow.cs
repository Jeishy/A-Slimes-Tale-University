using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityFireGlow : MonoBehaviour
{

    private AbilityManager _abilityManager;

    private void OnEnable()
    {
        Setup();
        _abilityManager.OnFireState += SpawnProjectile;
    }

    private void OnDisable()
    {
        _abilityManager.OnFireState -= SpawnProjectile;
    }

    private void Setup()
    {
        _abilityManager = GetComponent<AbilityManager>();
    }

    private void SpawnProjectile()
    {
      
    }
}
