using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityFireGlow : MonoBehaviour
{
    [SerializeField] private GameObject _fireGlowLight;

    private AbilityManager _abilityManager;

    private void OnEnable()
    {
        Setup();
        _abilityManager.OnFireState += FireGlowOn;
        _abilityManager.OnWaterState += FireGlowOff;
        _abilityManager.OnEarthCrash += FireGlowOff;
        _abilityManager.OnWindState += FireGlowOff;
    }

    private void OnDisable()
    {
        _abilityManager.OnFireState -= FireGlowOn;
        _abilityManager.OnWaterState -= FireGlowOff;
        _abilityManager.OnEarthCrash -= FireGlowOff;
        _abilityManager.OnWindState -= FireGlowOff;
    }

    private void Setup()
    {
        _abilityManager = GetComponent<AbilityManager>();
    }

    private void FireGlowOn()
    {
        _fireGlowLight.SetActive(true);
    }

    private void FireGlowOff()
    {
        if (_fireGlowLight.activeInHierarchy)
            _fireGlowLight.SetActive(false);
    }
}
