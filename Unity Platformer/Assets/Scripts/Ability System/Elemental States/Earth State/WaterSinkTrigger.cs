using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSinkTrigger : MonoBehaviour
{
    [SerializeField] private float _newBuoyancyDensity;

    private AbilityManager _abilityManager;
    private Rigidbody2D _playerRB;
    private BuoyancyEffector2D _buoyancyEffector;
    private float _originalBuoyancyDensity;
    private bool _isWaterEnter;

    private void Start()
    {
        _abilityManager = GameObject.FindGameObjectWithTag("AbilityManager").GetComponent<AbilityManager>();
        _playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        _buoyancyEffector = GetComponent<BuoyancyEffector2D>();
        _originalBuoyancyDensity = _buoyancyEffector.density;
        _isWaterEnter = false;
    }

	// Use this for initialization
    private void OnTriggerEnter2D(Collider2D col)
    {
        // If water is entered, reduce buoyancy density
        if (col.CompareTag("Player") && _abilityManager.CurrentPlayerElementalState == ElementalStates.Earth && !_isWaterEnter)
        {
            _isWaterEnter = true;
            _buoyancyEffector.density = _newBuoyancyDensity;
        }
        else if (col.CompareTag("Player") && _abilityManager.CurrentPlayerElementalState != ElementalStates.Earth)
        {
            _buoyancyEffector.density = _originalBuoyancyDensity;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _buoyancyEffector.density = _originalBuoyancyDensity;
            _isWaterEnter = false;
        }
    }
}
