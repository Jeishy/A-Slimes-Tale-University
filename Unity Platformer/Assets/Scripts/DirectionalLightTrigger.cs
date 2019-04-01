using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalLightTrigger : MonoBehaviour {

    [SerializeField][Range(0.01f, 1.0f)] private float _maxFadeTime;

    private bool _isEntered;
    private float _exposureAmount;

    private void Start()
    {
        _exposureAmount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isEntered)
        {
            _isEntered = true;
            StartCoroutine(FadeToDim());
        }
        else if (other.CompareTag("Player") && _isEntered)
        {
            _isEntered = false;
            StartCoroutine(FadeBack());
        }
    }

    private IEnumerator FadeToDim()
    {
        float elapsedTime = 0f;
        float originalExposure = RenderSettings.skybox.GetFloat("_Exposure");
        float currentExposure = 0f;
        while (elapsedTime <= _maxFadeTime)
        {
            Debug.Log("Fading");
            elapsedTime += Time.deltaTime;
            currentExposure = Mathf.Lerp(originalExposure, 0.2f, elapsedTime);
            Debug.Log(currentExposure);
            RenderSettings.skybox.SetFloat("_Exposure", currentExposure);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator FadeBack()
    {
        float elapsedTime = 0f;
        float originalExposure = RenderSettings.skybox.GetFloat("_Exposure");
        float currentExposure = 0f;
        while (elapsedTime <= _maxFadeTime)
        {
            Debug.Log("Fading");
            elapsedTime += Time.deltaTime;
            currentExposure = Mathf.Lerp(0.2f, originalExposure, elapsedTime);
            Debug.Log(currentExposure);
            RenderSettings.skybox.SetFloat("_Exposure", currentExposure);
            yield return new WaitForEndOfFrame();
        }
    }
}
