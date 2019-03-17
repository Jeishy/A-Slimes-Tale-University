using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySetNoneState : MonoBehaviour
{
    private AbilityManager _abilityManager;
    private Rigidbody2D _playerRb;

	// Use this for initialization
	private void OnEnable ()
    {
		Setup();
        _abilityManager.OnNoneState += SetNoneState;

    }
	
	// Update is called once per frame
	private void OnDisable ()
    {
        _abilityManager.OnNoneState -= SetNoneState;
    }

    private void Setup()
    {
        _abilityManager = GetComponent<AbilityManager>();
        _playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    private void SetNoneState()
    {
        Debug.Log("Setting state to none");
        _abilityManager.CurrentPlayerElementalState = ElementalStates.None;
        if (_playerRb.gravityScale < 3f)
            _playerRb.gravityScale = 3f;
    }
}
