using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthBreakablePlatform : MonoBehaviour {

    [SerializeField] private GameObject _platformDestroyedPE;

    private AbilityEarthCrash _abilityEarthCrash;
	private void Start()
	{
		_abilityEarthCrash = GameObject.FindGameObjectWithTag("AbilityManager").GetComponent<AbilityEarthCrash>();
	}

	private void OnTriggerEnter(Collider col)
	{
		if (col.CompareTag("Player") && _abilityEarthCrash.IsCrashAbilityActivated)
		{
            // Play particle effect or animation
            GameObject particle = Instantiate(_platformDestroyedPE, transform.position, Quaternion.identity);
            Destroy(particle, 1.1f);
            Destroy(gameObject);
            col.GetComponent<Rigidbody>().velocity = _abilityEarthCrash.InitialVelocity;
		}
	}
}
