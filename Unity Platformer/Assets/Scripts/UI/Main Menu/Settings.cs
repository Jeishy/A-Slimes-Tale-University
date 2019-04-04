using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {

    private AudioManager _audioManager;
    private float _origVolume;

    private void Start()
    {
        _audioManager = AudioManager.instance;
        _origVolume = AudioListener.volume;
    }

    public void OnMusicToggle(bool toggle)
    {
        if (!toggle)
            AudioListener.volume = 0f;
        else
            AudioListener.volume = _origVolume;
    }

    public void OnMasterVolumeChange(float volume)
    {
        AudioListener.volume = volume;
        Debug.Log(AudioListener.volume);
    }

    public void OnMusicVolumeChange(float volume)
    {

    }
}
