using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    [SerializeField] Dialogue dialogue;
	public void TriggerDialogue ()
    {
        // Start dialogue, parsing in dialogue to be displayed
        DialogueManager.Instance.StartDialogue(dialogue);
	}
}
