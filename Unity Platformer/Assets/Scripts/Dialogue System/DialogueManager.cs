using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    [SerializeField] private Text _nameText;
    [SerializeField] private Text _dialogueText;
    [SerializeField] [Range(0.001f, 0.2f)] private float _slowTypeSpeed;             // How slow dialogue text appears on the screen. Decrease to slow the effect
    [SerializeField] private Animator _anim;

    [HideInInspector] public static DialogueManager Instance = null;
    private Queue<string> _sentences;

	private void Awake()
    {
        if (Instance == null)
            Instance = this;

        _sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        // Animate text bubble onto screen
        _anim.SetBool("IsOpen", true);

        // Set name of text bubble to the character name of the dialogue variable
        _nameText.text = dialogue.characterName;

        // Clear any previously queued sentences
        _sentences.Clear();

        // Set sentences in dialogue to the sentences queue
        foreach (string sentence in dialogue.sentences)
        {
            _sentences.Enqueue(sentence);
        }

        // Display all the sentences in order of what is in the queue
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (_sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = _sentences.Dequeue();
        // Stop all coroutines that may be running
        StopAllCoroutines();
        // Start the slow typing coroutine
        StartCoroutine(SentenceSlowType(sentence));
    }

    private IEnumerator SentenceSlowType(string sentence)
    {
        // Appends sentence to dialogue text UI element character by character, pausing after slowTypeSpeed seconds
        _dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            _dialogueText.text += letter;
            yield return new WaitForSeconds(_slowTypeSpeed);
        }
    }

    private void EndDialogue()
    {
        // End the conversation
        // Animate text bubble off screen
        _anim.SetBool("IsOpen", false);
    }
}
