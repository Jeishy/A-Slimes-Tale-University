using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialougeTriggerVolume : MonoBehaviour {

    [SerializeField] private bool _waitToTrigger;
    [SerializeField] private bool _waitToLand;
    [SerializeField] private float _timeToWait;

    private DialogueTrigger _dialogueTrigger;
    private CharacterController2D _charController;
    private bool _canWaitToLand;

    private void Start()
    {
        _dialogueTrigger = GetComponent<DialogueTrigger>();
        _charController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
        _canWaitToLand = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_waitToTrigger && !_waitToLand)
            {
                _dialogueTrigger.TriggerDialogue();
                gameObject.SetActive(false);
            }
            else if (_waitToTrigger)
                StartCoroutine(WaitToCollect());
            else if (_waitToLand)
                _canWaitToLand = true;
        }
    }

    private IEnumerator WaitToCollect()
    {
        yield return new WaitForSeconds(_timeToWait);
        _dialogueTrigger.TriggerDialogue();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_canWaitToLand)
        {
            if (_charController.m_Grounded)
            {
                _dialogueTrigger.TriggerDialogue();
                _canWaitToLand = false;
                gameObject.SetActive(false);
            }
        }
    }
}
