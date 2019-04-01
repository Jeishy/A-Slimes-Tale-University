using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    [SerializeField] Dialogue _dialogue;
	public void TriggerDialogue ()
    {
        Debug.Log(_dialogue.name);
        // Start dialogue, parsing in dialogue to be displayed
        DialogueManager.Instance.StartDialogue(_dialogue);
	}
}
