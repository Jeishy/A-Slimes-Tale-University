using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAndHideInteractable : MonoBehaviour {

    [HideInInspector] public bool IsInteractable;

    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Light _pointLight;

    private float _originalLightRange;
    private float _originalRate;
    private float _originalRadius;
    private ParticleSystem.ShapeModule shapeModule;
    private ParticleSystem.EmissionModule emissionModule;

    private void Start()
    {
        IsInteractable = false;

        shapeModule = _particleSystem.shape;
        emissionModule = _particleSystem.emission;

        // Get reference to old radius, emission rate and light range values
        _originalRadius = emissionModule.rateOverTime.constant;
        _originalRate = shapeModule.radius;
        _originalLightRange = _pointLight.range;
    }

    private void Update()
    {
        if (IsInteractable)
        {
            _pointLight.range = Mathf.Lerp(_originalLightRange, _originalLightRange + 12, Time.deltaTime);
            if (_pointLight.range >= 13)
                IsInteractable = false;
        }
    }

    public void ShowInteractable()
    {
        // Set new radius and emission rate values
        shapeModule.radius = 3.236605f;
        emissionModule.rateOverTime = 200f;

        IsInteractable = true;
    }

    public void HideInteractable()
    {
        // Set original radius and emission rate values
        shapeModule.radius = _originalRadius;
        emissionModule.rateOverTime = _originalRate;
        _pointLight.range = _originalLightRange;
    }
}
