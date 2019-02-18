using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthBreakablePlatform : MonoBehaviour {
	[SerializeField] private float timeTillDestroyed;
	private AbilityEarthCrash abilityEarthCrash;
	private void Start()
	{
		abilityEarthCrash = GameObject.Find("AbilityManager").GetComponent<AbilityEarthCrash>();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Collider2D col = collision.gameObject.GetComponent<Collider2D>();
		if (col.CompareTag("Player") && abilityEarthCrash.CanDoSplashDamage)
		{
			// Play particle effect or animation
			Destroy(gameObject, timeTillDestroyed);
		}
	}
}
