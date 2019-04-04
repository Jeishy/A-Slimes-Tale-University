using UnityEngine.Audio;
using UnityEngine;
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 3f)]
    public float volume;
    [Range(0f, 1f)]
    public float pitch;
    public bool looping;
    [HideInInspector] // populated automatically in AudioManager.Awake
    // add an audio source that all attributes will be copied from
    
    public AudioSource source;
    
}