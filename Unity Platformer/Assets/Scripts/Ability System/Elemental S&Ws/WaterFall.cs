using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFall : MonoBehaviour {

    [SerializeField] private float _downwardForce;
    [SerializeField] private Collider _solidCol;

    private AbilityManager _abilityManager;
    private Player _player;

    // Use this for initialization
    private void Start () {
        _abilityManager = GameObject.FindGameObjectWithTag("AbilityManager").GetComponent<AbilityManager>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
	
	private void OnTriggerEnter(Collider col)
	{
		if (col.CompareTag("Player"))
		{
            ElementalStates state = _abilityManager.CurrentPlayerElementalState;
			Rigidbody rb = col.GetComponent<Rigidbody>();

            switch (state)
			{
				case ElementalStates.Earth:
                    rb.AddForce(0f, _downwardForce, 0f, ForceMode.Acceleration);
                    break;
				case ElementalStates.Water:
                    Debug.Log("Entered water fall as water state");
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
                    // Remove armour slot
					_solidCol.isTrigger = false;
                    Debug.Log("Fire state in waterfall");
                    _player.RemoveArmourSlot();
                    break;


            }
        }
	}
}
