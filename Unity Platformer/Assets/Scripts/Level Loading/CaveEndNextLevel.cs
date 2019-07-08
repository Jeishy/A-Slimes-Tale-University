using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveEndNextLevel : MonoBehaviour
{
    private bool _isNextLevel;
    private bool _hasActivatedNextLevel;
    private DialogueTrigger _dialogueTrigger;
    private bool _nextLevelDialogueStarted;
    private HunterMovement _hunterMovement;

    private void Start()
    {
        _hunterMovement = FindObjectOfType<HunterMovement>();
        _isNextLevel = false;
        _nextLevelDialogueStarted = false;
        _hasActivatedNextLevel = false;
        _dialogueTrigger = GetComponent<DialogueTrigger>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Hunter") && !_isNextLevel)
        {
            _hunterMovement.DisableMovement();
            _isNextLevel = true;
            TriggerDialouge();
        }
    }

    private void Update()
    {
        if (_nextLevelDialogueStarted)
        {
            if (!DialogueManager.Instance.IsDialogueRunning && !_hasActivatedNextLevel)
            {
                NextLevel();
                _hasActivatedNextLevel = true;
            }
        }
    }

    private void TriggerDialouge()
    {
        if (_dialogueTrigger._dialogue != null)
        {
            _dialogueTrigger.TriggerDialogue();
            _nextLevelDialogueStarted = true;
        }
        else
            Debug.LogError("There is no dialogue attatched to this trigger.");
    }


    private void NextLevel()
    {
        // Get hubworld build index and load the hub world
        int buildIndex = SceneUtility.GetBuildIndexByScenePath(GameManager.instance.GetScenePath("Hub_World"));
        LevelChanger.instance.FadeToLevel(buildIndex);
        // Reset variables
        _isNextLevel = false;
        _hasActivatedNextLevel = false;
    }
}
