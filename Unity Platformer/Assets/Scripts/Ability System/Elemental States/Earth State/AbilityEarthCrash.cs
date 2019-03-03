using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityEarthCrash : MonoBehaviour {
	/*private AbilityManager abilityManager;
	private Rigidbody2D playerRB;*/
	[SerializeField] private float downwardForce;
	[SerializeField] private float maxDamage;
	[SerializeField] private int timeAbilityActive;
    [SerializeField] private float enemyKnockbackForce;

    private AbilityManager abilityManager;
    private PlayerDurability playerDur;
    private Rigidbody2D playerRB;
    //Added lines of code just to fix compile errors. Delete if necessary.
    public bool IsCrashAbilityActivated;
    public Vector2 InitialVelocity;

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
        

        if (playerDur.armour >= 3)
        {
            // Armour slot is removed when ability is used
            playerDur.RemoveArmourSlot();
            playerRB.AddForce(new Vector2(0, -downwardForce), ForceMode2D.Impulse);
            IsCrashAbilityActivated = true;
            StartCoroutine(WaitTimeToActivateAbility());
            InitialVelocity = playerRB.velocity;
        }
        else
        {
            // Update UI: out of armour slots
            Debug.Log("You are out of armour slots!");
        }

    }

	public void SplashDamage(Collider2D[] enemyCols)
	{
		foreach (Collider2D enemyCol in enemyCols)
		{
			Vector2 enemyPos = enemyCol.transform.position;
			Vector2 playerPos = PlayerAttributes.Instance.playerTransform.position;

			// Get distance between player and enemy in range
			float distance = Vector2.Distance(playerPos, enemyPos);
            // Get direction from player to enemy
            Vector2 direction = enemyPos - playerPos;
            // Normalize the direction vector
            direction = direction.normalized;
            // Get enemies rigidbody2D component
            Rigidbody2D enemyRB = enemyCol.GetComponent<Rigidbody2D>();

            // Apply force to enemies in range
            enemyRB.AddForce(direction * enemyKnockbackForce, ForceMode2D.Impulse);
			// Do higher damage the closer an enemy is
            float damage = (1f / distance) * maxDamage;
			Debug.Log("Damage done to enemy " + enemyCol.name + ": " + damage);
			// Reduce health of enemy using damage variable
		}
	}

	private IEnumerator WaitTimeToActivateAbility()
	{
		yield return new WaitForSeconds(timeAbilityActive);

        if (IsCrashAbilityActivated)
            IsCrashAbilityActivated = false;
    }
}
