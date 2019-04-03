using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelDoor : MonoBehaviour {

	[SerializeField] private GameObject _levelChangePE;
    [SerializeField] private bool _canShowDialogue;

    private bool _isNextLevel;
    private bool _hasActivatedNextLevel;
    private DialogueTrigger _dialogueTrigger;
    private bool _nextLevelDialogueStarted;
    private GameObject _player;

    private void Start()
	{
        _isNextLevel = false;
        _nextLevelDialogueStarted = false;
        _hasActivatedNextLevel = false;
        _dialogueTrigger = GetComponent<DialogueTrigger>();
    }

    private void OnTriggerEnter(Collider col)
    {
        GameManager.instance.CheckIfLevelIsComplete();
        if (col.CompareTag("Player") && !_isNextLevel && GameManager.instance.IsLevelComplete)
		{
            _player = col.gameObject;
            _isNextLevel = true;
            if (_canShowDialogue)
                TriggerDialouge();
            else
                StartCoroutine(NextLevel(_player, _levelChangePE));
		}
	}

    private void Update()
    {
        if (_nextLevelDialogueStarted)
        {
            if (!DialogueManager.Instance.IsDialogueRunning && !_hasActivatedNextLevel)
            {
                StartCoroutine(NextLevel(_player, _levelChangePE));
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


	private IEnumerator NextLevel(GameObject player, GameObject nextLevelPoofPE)
	{	
		// Wait to spawn particle effect
		yield return new WaitForSeconds(0.2f);
		// Spawn and destroy particle effect
		GameObject nextLevelPoof = Instantiate(nextLevelPoofPE, player.transform.position, Quaternion.identity);
		// Deactive player GO to give illusion that player has teleported
		player.SetActive(false);
		Destroy(nextLevelPoof, 1f);
		// Delay before going to the next level
		yield return new WaitForSeconds(0.5f);
        LevelChanger.instance.OnLevelComplete();
		// Delay before reactivating player GO, so that player doesn't reappear during fade out animation
        yield return new WaitForSeconds(1f);
        player.SetActive(true);
		// Reset variables
        _isNextLevel = false;
        GameManager.instance.IsLevelComplete = false;
        GameManager.instance.gemstones = 0;
        _hasActivatedNextLevel = false;
    }
}
