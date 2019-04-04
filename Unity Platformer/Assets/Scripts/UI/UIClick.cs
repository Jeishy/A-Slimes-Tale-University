using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIClick : MonoBehaviour {

    private AudioManager _audioManager;

    private void Start()
    {
        _audioManager = AudioManager.instance;
    }

    public void PlayUIClickSound()
    {
        _audioManager.Play("UIClick");
    }
}
