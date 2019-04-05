using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour {

    [HideInInspector] public bool IsDialogueRunning;

    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] [Range(0.001f, 0.2f)] private float _slowTypeSpeed;             // How slow dialogue text appears on the screen. Decrease to slow the effect
    [SerializeField] private Animator _anim;
    [SerializeField] private GameObject _continueTxt;
    [SerializeField] private Transform _groundCheck;

    [HideInInspector] public static DialogueManager Instance = null;
    private Queue<string> _sentences;
    private PlayerControls _playerControls;
    private float _originalSpeed;
    private bool _isSentenceRunning;
    private bool _isSentenceSkipped;
    private bool _canDialogueEnd;
    private AbilityManager _abilityManager;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        _sentences = new Queue<string>();
    }

    private void Start()
    {
        _playerControls = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>();
        _abilityManager = GameObject.FindGameObjectWithTag("AbilityManager").GetComponent<AbilityManager>();
        IsDialogueRunning = false;
        _isSentenceSkipped = false;
        _isSentenceRunning = false;
        _canDialogueEnd = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && IsDialogueRunning && _isSentenceRunning)
            _isSentenceSkipped = true;

        else if (Input.GetMouseButtonDown(0) && !_isSentenceRunning && _canDialogueEnd)
        {
            IsDialogueRunning = false;
            EndDialogue();
        }
        else if (Input.GetMouseButtonDown(0) && !_isSentenceRunning)
            DisplayNextSentence();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        // Set dialogue and sentence started flag to true
        IsDialogueRunning = true;
        _isSentenceRunning = true;
        // Animate text bubble onto screen
        _anim.SetTrigger("Open");
        // Set name of text bubble to the character name of the dialogue variable
        _nameText.text = dialogue.characterName;
        // Cache original player speed
        _originalSpeed = _playerControls.GetSpeed();
        // Reduce movement speed to 0
        _playerControls.SetSpeed(0);
        // Disable shooting if there is an ability manager in the scene
        if (_abilityManager != null)
            _abilityManager.GetComponent<AbilityProjectile>().enabled = false;
        // Disable jumping by moving ground check gameobject off the ground
        _groundCheck.position = new Vector3(_groundCheck.position.x, _groundCheck.position.y + 1, _groundCheck.position.z);

        // Deactivate continue text during sentence
        _continueTxt.SetActive(false);
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
        _isSentenceRunning = true;
        // If sentence was skipped, reset sentence skipped flag
        if (_isSentenceSkipped == true)
            _isSentenceSkipped = false;

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
            if (!_isSentenceSkipped)
            {
                _dialogueText.text += letter;
                yield return new WaitForSeconds(_slowTypeSpeed);       
            }
            else
            {
                _dialogueText.text = sentence;
            }
        }
        // Activate continue text after sentence
        _continueTxt.SetActive(true);
        _isSentenceRunning = false;

        if (_sentences.Count == 0 && !_isSentenceRunning)
        {
            _canDialogueEnd = true;
            yield return null;
        }
    }

    private void EndDialogue()
    {
        // Set players movement speed back to normal
        _playerControls.SetSpeed(_originalSpeed);
        // Renable shooting if the ability manager is in the scene
        if (_abilityManager != null)
            _abilityManager.GetComponent<AbilityProjectile>().enabled = true;
        // Renable jumping
        _groundCheck.position = new Vector3(_groundCheck.position.x, _groundCheck.position.y - 1, _groundCheck.position.z);
        // End the conversation
        // Reset all flags 
        IsDialogueRunning = false;
        _isSentenceRunning = false;
        _isSentenceSkipped = false;
        _canDialogueEnd = false;
        // Animate text bubble off screen
        _anim.SetTrigger("Close");
    }
}
