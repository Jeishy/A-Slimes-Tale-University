using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityEarthCrash : MonoBehaviour {
	private AbilityManager abilityManager;
	private Rigidbody2D playerRB;
	[SerializeField] private float downwardForce;
	[SerializeField] private float maxDamage;
	[SerializeField] private int timeToDoDamage;
	public float splashRadius;
	[HideInInspector] public bool CanDoSplashDamage;
	
	private void OnEnable()
	{
		Setup();
		abilityManager.OnEarthCrash += EarthCrash;
	}

	private void OnDisable()
	{
		abilityManager.OnEarthCrash -= EarthCrash;		
	}

	private void Setup()
	{
		abilityManager = GetComponent<AbilityManager>();
        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
	}

	private void EarthCrash()
	{
		playerRB.AddForce(new Vector2(0, -downwardForce), ForceMode2D.Impulse);
		CanDoSplashDamage = true;
		StartCoroutine(WaitTimeToDoDamage());
	}

	public void SplashDamage(Collider2D[] enemyCols)
	{
		Debug.Log("Doing splash damage");
		foreach (Collider2D enemyCol in enemyCols)
		{
			Vector2 enemyPos = enemyCol.transform.position;
			Vector2 playerPos = PlayerAttributes.Instance.playerTransform.position;
			// Get distance between player and enemy in range
			float distance = Vector2.Distance(playerPos, enemyPos);
			// Do higher damage the closer an enemy is
			float damage = (1f / distance) * maxDamage;
			Debug.Log("Damage done to enemy " + enemyCol.name + ": " + damage);
			// Reduce health of enemy using damage variable
		}
	}

	private IEnumerator WaitTimeToDoDamage()
	{
		yield return new WaitForSeconds(timeToDoDamage);

		// Set bool so false only if it isnt already false
		if (CanDoSplashDamage)
			CanDoSplashDamage = false;
	}
}
