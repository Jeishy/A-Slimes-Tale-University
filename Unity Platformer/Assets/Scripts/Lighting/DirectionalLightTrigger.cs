using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalLightTrigger : MonoBehaviour
{
    // Directional lights in the scene, as well as the point light for on fire state glow
    [SerializeField] private Light _fireGlowLight;
    [SerializeField] private Light _mainLight;
    [SerializeField] private Light _sideLight;
    [SerializeField] private Light _tintLight;
    [SerializeField][Range(0.01f, 5.0f)] private float _maxFadeTime;

    private bool _isEntered;
    private float _fadeTime;
    private float _origMainLight;
    private float _origSideLight;
    private float _origTintLight;


    private void Start()
    {
        // Initialise variables
        _isEntered = false;
        _fadeTime = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isEntered)
        {
            _isEntered = true;
            StopAllCoroutines();
            StartCoroutine(DimLights());
            // Increase fire glow when in dark areas
            _fireGlowLight.intensity = 31f;
        }
        else if (other.CompareTag("Player") && _isEntered)
        {
            _isEntered = false;
            StopAllCoroutines();
            StartCoroutine(BrightenLights());
            // Set fire glow intensity back to normal when exiting dark area
            _fireGlowLight.intensity = 5f;
        }
    }

    private IEnumerator DimLights()
    {
        _origMainLight = _mainLight.intensity;
        _origSideLight = _sideLight.intensity;
        _origTintLight = _tintLight.intensity;

        while (_fadeTime <= _maxFadeTime)
        {
            _fadeTime += Time.deltaTime;
            // Lerp between original and new intensity
            float newMain = Mathf.Lerp(_origMainLight, 0f, _fadeTime);
            float newSide = Mathf.Lerp(_origSideLight, 0f, _fadeTime);
            float newTint = Mathf.Lerp(_origTintLight, 0.1f, _fadeTime);
            // Set intensity of lights
            _mainLight.intensity = newMain;
            _sideLight.intensity = newSide;
            _tintLight.intensity = newTint;
            yield return new WaitForEndOfFrame();
        }

        _fadeTime = 0f;
    }

    private IEnumerator BrightenLights()
    {
        float currentMainIntensity = _mainLight.intensity;
        float currentSideIntensity = _sideLight.intensity;
        float currentTintIntensity = _tintLight.intensity;

        while (_fadeTime <= _maxFadeTime)
        {
            _fadeTime += Time.deltaTime;
            Debug.Log("fade time is: " + _fadeTime);
            // Lerp between current and original intensity
            float newMain = Mathf.Lerp(currentMainIntensity, _origMainLight, _fadeTime);
            float newSide = Mathf.Lerp(currentSideIntensity, _origSideLight, _fadeTime);
            float newTint = Mathf.Lerp(currentTintIntensity, _origTintLight, _fadeTime);
            // Set intensity of lights
            _mainLight.intensity = newMain;
            _sideLight.intensity = newSide;
            _tintLight.intensity = newTint;
            yield return new WaitForEndOfFrame();
        }
        // Reset fade time
        _fadeTime = 0f;
    }
}
