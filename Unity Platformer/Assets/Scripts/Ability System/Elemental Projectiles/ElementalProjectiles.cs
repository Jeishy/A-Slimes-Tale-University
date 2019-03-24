using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class that all elemental projectiles inherit from
public class ElementalProjectiles : MonoBehaviour {

    public float ProjectileSpeed;
    public float TimeTillDestroy;

    // public AudioClip projectileSound; uncomment when sounds are added
    [HideInInspector] public bool IsProjectileSpawned;
    [HideInInspector] public bool IsBoosted;
    [HideInInspector] public AbilityManager abilityManager;
    [HideInInspector] public Transform playerTrans;
    [HideInInspector] public ElementalStates projectileElementalState;

    [SerializeField] private float _dropOffTime;

    private Vector2 _projForce;
    private Vector2 _hitPoint;

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
        _hitPoint = ray.GetPoint(enter);
        // Get direction between mouse and player
        Vector2 playerPos = playerTrans.position;
        Vector2 direction = _hitPoint - playerPos;
        direction = direction.normalized;
        // Change velocity of projectile to calculated normalized direction vector * specified speed (magnitude)
        _projForce = direction * projectileSpeed;
        return _projForce;
    }

    // Note, elemental projectile prefabs have rigidbodies on them
    public virtual Vector2 JoystickFiringForce(float projectileSpeed, Transform playerTrans, Vector2 joystickDir)
    {
        // Set force to be the direction the right joystick is held in * the speed of the projectile
        Vector2 projForce = joystickDir * projectileSpeed;
        return projForce;
    }

    public virtual IEnumerator GravityDropOff(Rigidbody proj)
    {
        yield return new WaitForSeconds(_dropOffTime);
        proj.useGravity = true;
	}

    // Does flat damage to hit enemy
    public virtual void FlatDamageToEnemy(float damage, Collider enemyCol)
    {
        ElementalStates enemyElementalState = enemyCol.gameObject.GetComponent<EnemyDurability>().element;
        // Reduce enemy's health by baseDamage
        enemyCol.GetComponent<EnemyAI>().Hit(CalculateDamage(projectileElementalState, enemyElementalState, damage));
    }

    // Does DOT to hit enemy
    public virtual IEnumerator DotToEnemy(float initialDmg, float dot, int dotTime, GameObject proj, Collider enemyCol)
    {
        ElementalStates enemyElementalState = enemyCol.GetComponent<EnemyDurability>().element;

        // Do intial damage to enemy with intialDmg
        enemyCol.GetComponent<EnemyAI>().Hit(CalculateDamage(projectileElementalState, enemyElementalState, initialDmg));
        // Apply dot over time, till dotTime is elapsed
        for (int i = 0; i < dotTime; i++)
        {
            yield return new WaitForSeconds(1f);
            // Do damage to enemy
            enemyCol.GetComponent<EnemyAI>().Hit(dot);
        }
        Destroy(gameObject);
    }

    // Does knockback damage to hit enemy
    public virtual void KnockbackDamageToEnemy(float damage, float knockbackForce, Transform projTrans, Collider enemyCol)
    {
        ElementalStates enemyElementalState = enemyCol.GetComponent<EnemyDurability>().element;

        enemyCol.GetComponent<EnemyAI>().Hit(CalculateDamage(projectileElementalState, enemyElementalState, damage));
        Vector2 enemyPos = enemyCol.transform.position;
        Vector2 projPos = projTrans.position;
        Vector2 dir = Vector3.Normalize(enemyPos - projPos);
        // Get x component of direciton vector
        Vector2 dirInX = new Vector3(dir.x, 0);
        Rigidbody enemyRb = enemyCol.GetComponent<Rigidbody>();

        // Reduce enemy's health based on damage amount 
        // Only apply forces in x direction
        enemyRb.AddForce(dirInX * knockbackForce, ForceMode.Impulse);
    }

    private float CalculateDamage(ElementalStates projectileElementalState, ElementalStates otherElementalState, float dmg)
    {
        float damage = dmg;
        if ((projectileElementalState == ElementalStates.Fire && otherElementalState == ElementalStates.Earth) ||
            (projectileElementalState == ElementalStates.Earth && otherElementalState == ElementalStates.Wind) ||
            (projectileElementalState == ElementalStates.Wind && otherElementalState == ElementalStates.Water) ||
            (projectileElementalState == ElementalStates.Water && otherElementalState == ElementalStates.Fire))
        {
            // Apply damage multiplier
            damage = damage * abilityManager.PlayerElementalDmgMultiplier;
        }
        /*else if (projectileElementalState == otherElementalState)
        {
            // OPTIONAL: Do less damage when 
            damage *= 
        }*/
        return damage;
    }
}
