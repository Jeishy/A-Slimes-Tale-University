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

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Collider2D col = collision.gameObject.GetComponent<Collider2D>();
		if (col.CompareTag("Player") && _abilityEarthCrash.IsCrashAbilityActivated)
		{
            col.GetComponent<Rigidbody2D>().velocity = _abilityEarthCrash.InitialVelocity;
			// Play particle effect or animation
			Destroy(gameObject, _timeTillDestroyed);
		}
	}
}
