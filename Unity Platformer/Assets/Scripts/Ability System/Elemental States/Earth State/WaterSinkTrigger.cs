using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSinkTrigger : MonoBehaviour
{
    [SerializeField][Range(0.1f, 100.0f)] private float _resistiveForce;
    [SerializeField] private Collider _solidCollider;

    private AbilityManager _abilityManager;
    private Rigidbody _playerRB;
    private Vector3 _force;

    private void Start()
    {
        _abilityManager = GameObject.FindGameObjectWithTag("AbilityManager").GetComponent<AbilityManager>();
        _playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }

	// Use this for initialization
    private void OnTriggerEnter(Collider col)
    {
        _force = new Vector3(0f, _resistiveForce, 0f);
        if (col.CompareTag("Player") && _abilityManager.CurrentPlayerElementalState == ElementalStates.Earth)
        {
            Debug.Log("Adding upward force. ");
            _solidCollider.isTrigger = true;
            col.GetComponent<Rigidbody>().AddForce(_force);
        }
        else
        {
            _solidCollider.isTrigger = false;
        }
    }
}
