using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSinkTrigger : MonoBehaviour
{
    [SerializeField] private Collider _solidCollider;

    private AbilityManager _abilityManager;
    private Rigidbody2D _playerRB;

    private void Start()
    {
        _abilityManager = GameObject.FindGameObjectWithTag("AbilityManager").GetComponent<AbilityManager>();
        _playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

	// Use this for initialization
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") && _abilityManager.CurrentPlayerElementalState == ElementalStates.Earth)
        {
            _solidCollider.isTrigger = true;
        }
        else
        {
            _solidCollider.isTrigger = false;
        }
    }
}
