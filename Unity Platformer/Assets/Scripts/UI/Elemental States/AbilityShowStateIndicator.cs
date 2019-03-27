using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityShowStateIndicator : MonoBehaviour
{
    [Tooltip("Index order: 0 - Fire, 1 - Water, 2 - Wind, 3- Earth")]
    [SerializeField] private GameObject[] _elementalStateIndicators;

    private AbilityManager _abilityManager;
    
    // Use this for initialization
    private void OnEnable ()
    {
        Setup();
        _abilityManager.OnFireState += ShowStateIndicator;
        _abilityManager.OnWaterState += ShowStateIndicator;
        _abilityManager.OnWindState += ShowStateIndicator;
        _abilityManager.OnEarthState += ShowStateIndicator;
        _abilityManager.OnNoneState += DeactiveStateIndicators;
    }

    // Update is called once per frame
    private void OnDisable () {
        _abilityManager.OnFireState -= ShowStateIndicator;
        _abilityManager.OnWaterState -= ShowStateIndicator;
        _abilityManager.OnWindState -= ShowStateIndicator;
        _abilityManager.OnEarthState -= ShowStateIndicator;
        _abilityManager.OnNoneState -= DeactiveStateIndicators;
    }

    private void Setup()
    {
        _abilityManager = GameObject.FindGameObjectWithTag("AbilityManager").GetComponent<AbilityManager>();
    }

    private void ShowStateIndicator()
    {
        Debug.Log("Changing elemental indicator");
        StopAllCoroutines();    
        StartCoroutine(WaitToShowStateIndicator());
    }

    private void DeactiveStateIndicators()
    {
        // Set all state indicators that maybe active to inactive when a state is changed
        foreach (GameObject stateIndicator in _elementalStateIndicators)
        {
            if (stateIndicator.activeInHierarchy)
                stateIndicator.SetActive(false);
        }
    }

    private IEnumerator WaitToShowStateIndicator()
    {
        yield return new WaitForSeconds(0.05f);

        ElementalStates currentState = _abilityManager.CurrentPlayerElementalState;
        DeactiveStateIndicators();

        // Turn on the relevant state indicator to true
        switch (currentState)
        {
            case ElementalStates.Fire:
                _elementalStateIndicators[0].SetActive(true);
                break;
            case ElementalStates.Water:
                _elementalStateIndicators[1].SetActive(true);
                break;
            case ElementalStates.Wind:
                _elementalStateIndicators[2].SetActive(true);
                break;
            case ElementalStates.Earth:
                _elementalStateIndicators[3].SetActive(true);
                break;
        }
    }
}

