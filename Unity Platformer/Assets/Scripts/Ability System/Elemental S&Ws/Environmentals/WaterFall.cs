using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFall : MonoBehaviour {

    [SerializeField] private float _downwardForce;
    [SerializeField] private float _knockbackMultiplier;
    [SerializeField] private Collider _solidCol;
    [SerializeField] [Range(0.01f, 0.9f)] private float _moveSpeedReductionPercentage;
    private AbilityManager _abilityManager;
    private Player _player;
    private PlayerControls _playerControls;
    private float _originalSpeed;
    private bool _isSpeedChanged;

    // Use this for initialization
    private void Start () {
        _abilityManager = GameObject.FindGameObjectWithTag("AbilityManager").GetComponent<AbilityManager>();
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        _player = playerGO.GetComponent<Player>();
        _playerControls = playerGO.GetComponent<PlayerControls>();
        _originalSpeed = _playerControls.GetSpeed();
		_isSpeedChanged = false;
    }
	
	private void OnTriggerStay(Collider col)
	{
		if (col.CompareTag("Player"))
		{
            ElementalStates state = _abilityManager.CurrentPlayerElementalState;
			Rigidbody rb = col.GetComponent<Rigidbody>();
            switch (state)
			{
				case ElementalStates.Earth:
					_solidCol.isTrigger = true;
                    // Send player down stream if in earth state
                    Debug.Log("In waterfall in earth state");
                    rb.AddForce(0f, _downwardForce, 0f, ForceMode.Acceleration);
                    // Reduce movement speed
					if (!_isSpeedChanged)
					{
                        _isSpeedChanged = true;
						_playerControls.SetSpeed(_playerControls.GetSpeed() * _moveSpeedReductionPercentage);
                    }
                    break;
				case ElementalStates.Water:
					// Allow player to pass through waterfall if in water state
                    _solidCol.isTrigger = true;
                    break;
				case ElementalStates.Wind:
					_solidCol.isTrigger = false;
                    break;
				case ElementalStates.None:
					_solidCol.isTrigger = false;
					break;
				case ElementalStates.Fire:
                    // Play fire quenched particle effects
                    Vector3 vel = rb.velocity;
					// Knocback player if entered the waterfall in fire state 
                    rb.velocity = new Vector3(-vel.x * _knockbackMultiplier, vel.y, 0f);
                    _solidCol.isTrigger = false;
                    Debug.Log("Fire state in waterfall");
					// Remove armour slot
					// Set 
                    _player.RemoveArmourSlot();
                    break;
            }
        }
	}

	private void OnTriggerExit(Collider col)
	{
		if (col.CompareTag("Player"))
		{
			// Set speed changed bool back to false
			_isSpeedChanged = false;
			// Set waterfall's collision collider to work again
            _solidCol.isTrigger = false;
			// Set player's speed back to the original speed
            _playerControls.SetSpeed(_originalSpeed);
        }
	}
}
