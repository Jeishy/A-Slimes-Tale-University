using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthBreakablePlatform : MonoBehaviour {
	[SerializeField] private float _timeTillDestroyed;
	
	private AbilityEarthCrash _abilityEarthCrash;
	private void Start()
	{
		_abilityEarthCrash = GameObject.FindGameObjectWithTag("AbilityManager").GetComponent<AbilityEarthCrash>();
	}

	private void OnTriggerEnter(Collider collision)
	{
		Collider col = collision.gameObject.GetComponent<Collider>();
		if (col.CompareTag("Player") && _abilityEarthCrash.IsCrashAbilityActivated)
		{
            col.GetComponent<Rigidbody>().velocity = _abilityEarthCrash.InitialVelocity;
			// Play particle effect or animation
			Destroy(gameObject, _timeTillDestroyed);
		}
	}
}
