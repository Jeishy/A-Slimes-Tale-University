using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityEarthCrash : MonoBehaviour {

    public float SplashRadius;
    [HideInInspector] public Vector3 InitialVelocity;
    [HideInInspector] public bool IsCrashAbilityActivated;

    [SerializeField] private float _downwardForce;
	[SerializeField] private float _maxDamage;
	[SerializeField] private int _timeAbilityActive;
    [SerializeField] private float _enemyKnockbackForce;

    
    private Player _playerDur;
    private AbilityManager _abilityManager;
    private Rigidbody _playerRb;

    private void OnEnable()
	{
		Setup();
		_abilityManager.OnEarthCrash += EarthCrash;
	}

	private void OnDisable()
	{
		_abilityManager.OnEarthCrash -= EarthCrash;		
	}

	private void Setup()
	{
		_abilityManager = GetComponent<AbilityManager>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _playerRb = player.GetComponent<Rigidbody>();
        _playerDur = player.GetComponent<Player>();
    }

	private void EarthCrash()
	{
        if (_playerDur.armour >= 3)
        {
            // Armour slot is removed when ability is used
            _playerDur.RemoveArmourSlot();
            _playerRb.AddForce(new Vector2(0, -_downwardForce), ForceMode.Impulse);
            IsCrashAbilityActivated = true;
            StartCoroutine(WaitTimeToActivateAbility());
            InitialVelocity = _playerRb.velocity;
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
            Rigidbody2D enemyRb = enemyCol.GetComponent<Rigidbody2D>();

            // Apply force to enemies in range
            enemyRb.AddForce(direction * _enemyKnockbackForce, ForceMode2D.Impulse);
			// Do higher damage the closer an enemy is
            float damage = (1f / distance) * _maxDamage;
			Debug.Log("Damage done to enemy " + enemyCol.name + ": " + damage);
			// Reduce health of enemy using damage variable
		}
	}

	private IEnumerator WaitTimeToActivateAbility()
	{
		yield return new WaitForSeconds(_timeAbilityActive);

        if (IsCrashAbilityActivated)
            IsCrashAbilityActivated = false;
    }
}
