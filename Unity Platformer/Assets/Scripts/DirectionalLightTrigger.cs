using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalLightTrigger : MonoBehaviour {

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
        }
        else if (other.CompareTag("Player") && _isEntered)
        {
            _isEntered = false;
        }
    }

    private IEnumerator DimLights()
    {
        _origMainLight = _mainLight.intensity;
        _origSideLight = _sideLight.intensity;
        _origTintLight = _tintLight.intensity;

        while (_fadeTime <= _maxFadeTime)
        {
            Debug.Log("Diming lights. Fade time is: " + _fadeTime);
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
    }
}
