using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class that all elemental projectiles inherit from
public class ElementalProjectiles : MonoBehaviour {

	// public AudioClip projectileSound; uncomment when sounds are added
    [HideInInspector] public bool IsBoosted;
    [HideInInspector] public AbilityManager abilityManager;
    [HideInInspector] public Transform playerTrans;
	public float ProjectileSpeed;
    private Vector3 projForce;
    private Vector3 hitPoint;

    private void Awake()
    {
        IsBoosted = false;
    }

    public virtual void LoadPlayerVariables()
	{
		playerTrans = PlayerAttributes.Instance.playerTransform;
		abilityManager = GameObject.FindGameObjectWithTag("AbilityManager").GetComponent<AbilityManager>();
	}

    public virtual Vector3 AimToFireProjectileForce(float projectileSpeed, Ray ray, float enter, Transform playerTrans)
    {
        hitPoint = ray.GetPoint(enter);
        // Get direction between mouse and player
        Vector3 playerPos = playerTrans.position;
        Vector3 direction = Vector3.Normalize(hitPoint - playerPos);
        // Change velocity of projectile to calculated normalized direction vector * specified speed (magnitude)
        projForce = direction * projectileSpeed;
        return projForce;
    }

    // Note, elemental projectile prefabs have rigidbodies on them
    public virtual Vector3 FireProjectileForward(float projectileSpeed, Transform playerTrans)
    {
        // Get transform's forward direction
        Vector3 forwardDirection = playerTrans.forward;
        // Change velocity of projectile to forward direction vector * specified speed (magnitude)
        Vector3 projForce = forwardDirection * projectileSpeed;
        return projForce;
    }

    // Does flat damage to hit enemy
    public virtual void FlatDamageToEnemy(float damage, Collider enemyCol)
    {
        // Reduce enemy's health by baseDamage
    }

    // Does DOT to hit enemy
    public virtual IEnumerator DOTToEnemy(float dot, int dotTime, GameObject proj, Collider enemyCol)
    {
        proj.GetComponent<MeshRenderer>().enabled = false;
        // Apply dot over time, till dotTime is elapsed
        int dotCount = 0;
        while (dotCount < dotTime)
        {
            dotCount++;
            // Do damage to enemy
            Debug.Log("Damage done: " + dot);
            yield return new WaitForSeconds(1);
        }
        // This is done here rather than in OnTriggerEnter in the fireprojectile class
        // as the coroutine will stop execution when the gameobject is set to false
        proj.GetComponent<MeshRenderer>().enabled = true;
        proj.SetActive(false);
    }

    // Does knockback damage to hit enemy
    public virtual void KnockbackDamageToEnemy(float damage, float knockbackForce, Transform projTrans, Collider enemyCol)
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
