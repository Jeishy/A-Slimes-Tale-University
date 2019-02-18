using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class that all elemental projectiles inherit from
public class ElementalProjectiles : MonoBehaviour {

	// public AudioClip projectileSound; uncomment when sounds are added
    [HideInInspector] public bool IsBoosted;
    [HideInInspector] public AbilityManager abilityManager;
    [HideInInspector] public Transform playerTrans;
    [SerializeField] private int dropOffTime;
	public float ProjectileSpeed;
    private Vector2 projForce;
    private Vector2 hitPoint;

    private void Awake()
    {
        IsBoosted = false;
    }

    public virtual void LoadPlayerVariables()
	{
		playerTrans = PlayerAttributes.Instance.playerTransform;
		abilityManager = GameObject.FindGameObjectWithTag("AbilityManager").GetComponent<AbilityManager>();
	}

    public virtual Vector2 AimToFireProjectileForce(float projectileSpeed, Ray ray, float enter, Transform playerTrans)
    {
        hitPoint = ray.GetPoint(enter);
        // Get direction between mouse and player
        Vector2 playerPos = playerTrans.position;
        Vector2 direction = hitPoint - playerPos;
        direction = direction.normalized;
        // Change velocity of projectile to calculated normalized direction vector * specified speed (magnitude)
        projForce = direction * projectileSpeed;
        return projForce;
    }

    // Note, elemental projectile prefabs have rigidbodies on them
    public virtual Vector2 FireProjectileForward(float projectileSpeed, Transform playerTrans)
    {
        // Get transform's forward direction
        Vector2 forwardDirection = playerTrans.forward;
        // Change velocity of projectile to forward direction vector * specified speed (magnitude)
        Vector2 projForce = forwardDirection * projectileSpeed;
        return projForce;
    }
 
	public virtual IEnumerator GravityDropOff(Rigidbody2D proj)
	{
		yield return new WaitForSeconds(dropOffTime);
        proj.gravityScale = 3;
	}

    // Does flat damage to hit enemy
    public virtual void FlatDamageToEnemy(float damage, Collider2D enemyCol)
    {
        // Reduce enemy's health by baseDamage
    }

    // Does DOT to hit enemy
    public virtual IEnumerator DOTToEnemy(float dot, int dotTime, GameObject proj, Collider2D enemyCol)
    {
        // Apply dot over time, till dotTime is elapsed
        int dotCount = 0;
        while (dotCount < dotTime)
        {
            dotCount++;
            // Do damage to enemy
            Debug.Log("Damage done: " + dot);
            yield return new WaitForSeconds(1);
        }
    }

    // Does knockback damage to hit enemy
    public virtual void KnockbackDamageToEnemy(float damage, float knockbackForce, Transform projTrans, Collider2D enemyCol)
    {
        Vector3 enemyPos = enemyCol.transform.position;
        Vector3 projPos = projTrans.position;
        Vector3 direciton = Vector3.Normalize(enemyPos - projPos);
        // Get x component of direciton vector
        Vector3 directionInX = new Vector3(direciton.x, 0, 0);
        Rigidbody enemyRB = enemyCol.GetComponent<Rigidbody>();

        // Reduce enemy's health based on damage amount 
        // Only apply forces in x direction
        enemyRB.AddForce(directionInX * knockbackForce, ForceMode.Impulse);
    }
}
