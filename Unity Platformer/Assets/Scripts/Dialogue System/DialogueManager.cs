using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
    private HunterMovement _hunterMovement;
    private float _originalSpeed;
    private bool _isSentenceRunning;
    private bool _isSentenceSkipped;
    private bool _canDialogueEnd;
    private AbilityManager _abilityManager;
    private float _oldSpeed;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        _sentences = new Queue<string>();
    }

    private void Start()
    {
        _hunterMovement = FindObjectOfType<HunterMovement>();
        if (SceneManager.GetActiveScene().name != "Chapter_0")
        {
            _playerControls = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>();
            _abilityManager = GameObject.FindGameObjectWithTag("AbilityManager").GetComponent<AbilityManager>();
            _oldSpeed = _playerControls.GetSpeed();
        }
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
        if (SceneManager.GetActiveScene().name == "Chapter_0")
            ChapterZeroSetup();
        else
            NormalSetup();
    
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

    private void ChapterZeroSetup()
    {
        _hunterMovement.DisableMovement();
    }

    private void NormalSetup()
    {
        // Disable movement
        _playerControls.DisableMovement();
        // Disable shooting if there is an ability manager in the scene
        if (_abilityManager != null)
            _abilityManager.GetComponent<AbilityProjectile>().enabled = false;
    }
    
    private void ChapterZeroEnd()
    {
        _hunterMovement.EnableMovement();
    }

    private void NormalEnd()
    {
        // Renable movement
        _playerControls.EnableMovement();
        // Renable shooting if the ability manager is in the scene
        if (_abilityManager != null)
            _abilityManager.GetComponent<AbilityProjectile>().enabled = true;
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
        if (SceneManager.GetActiveScene().name == "Chapter_0")
            ChapterZeroEnd();
        else
            NormalEnd();

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
