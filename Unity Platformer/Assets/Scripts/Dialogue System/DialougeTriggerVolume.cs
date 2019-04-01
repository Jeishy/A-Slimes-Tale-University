using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialougeTriggerVolume : MonoBehaviour {

    private DialogueTrigger _dialogueTrigger;

    private void Start()
    {
        _dialogueTrigger = GetComponent<DialogueTrigger>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _dialogueTrigger.TriggerDialogue();
        }
    }
}
