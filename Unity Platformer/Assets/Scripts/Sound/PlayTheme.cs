using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTheme : MonoBehaviour {

    [SerializeField] private string _themeName;

    private void Start()
    {
        AudioManager.instance.StopAll();
        AudioManager.instance.Play(_themeName);
    }
}
