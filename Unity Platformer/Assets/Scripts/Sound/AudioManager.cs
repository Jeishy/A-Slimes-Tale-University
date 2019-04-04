using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    public static AudioManager instance = null;

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Two instances of AudioManager have been created!");
            Destroy(this);
        }

        // populate array attributes from sound source attributes
        foreach (Sound s in sounds)
            
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.looping;

            
        }
    }

    private void Start()
    {
        Play("ThemeDungeon");
    }


    public void Play(string name)
    {


        // find sound in sounds array where Sound.name is equal to name
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            // display sound not found error message
            Debug.LogWarning("[Warning] " + name + " sound not found");
            // don't execute
            return;
        }
        s.source.Play();
    }

    public void Stop(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            // display sound not found error message
            Debug.LogWarning("[Warning] " + name + " sound not found");
            // don't execute
            return;
        }
        s.source.Stop();
    }
    
    public void StopAll()
    {
        foreach (Sound sound in sounds)
        {
            sound.source.Stop();
        }
    }

}
