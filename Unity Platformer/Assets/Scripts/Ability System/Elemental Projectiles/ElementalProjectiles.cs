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
    [HideInInspector] public AbilityManager AbilityManager;
    [HideInInspector] public Transform PlayerTrans;
    [HideInInspector] public ElementalStates ProjectileElementalState;

    [SerializeField] private float _dropOffTime;

    private Vector2 _projForce;
    private Vector2 _hitPoint;

    private void Awake()
    {
        IsBoosted = false;
    }

    public virtual void LoadPlayerVariables()
	{
		PlayerTrans = PlayerAttributes.Instance.playerTransform;
		AbilityManager = GameObject.FindGameObjectWithTag("AbilityManager").GetComponent<AbilityManager>();
	}

    public virtual Vector2 AimToFireProjectileForce(float projectileSpeed, Ray ray, float enter, Transform PlayerTrans)
    {
        _hitPoint = ray.GetPoint(enter);
        // Get direction between mouse and player
        Vector2 playerPos = PlayerTrans.position;
        Vector2 direction = _hitPoint - playerPos;
        direction = direction.normalized;
        // Change velocity of projectile to calculated normalized direction vector * specified speed (magnitude)
        _projForce = direction * projectileSpeed;
        return _projForce;
    }

    public virtual IEnumerator GravityDropOff(Rigidbody proj)
    {
        yield return new WaitForSeconds(_dropOffTime);
        proj.useGravity = true;
	}

    // Does flat damage to hit enemy
    public virtual void FlatDamageToEnemy(float damage, Collider enemyCol)
    {
        ElementalStates enemyElementalState = enemyCol.GetComponent<EnemyAI>().Element;
        // Reduce enemy's health by baseDamage
        enemyCol.GetComponent<EnemyAI>().Hit(CalculateDamage(ProjectileElementalState, enemyElementalState, damage));
    }

    // Does DOT to hit enemy
    public virtual IEnumerator DotToEnemy(float initialDmg, float dot, int dotTime, GameObject proj, Collider enemyCol)
    {
        ElementalStates enemyElementalState = enemyCol.GetComponent<EnemyAI>().Element;

        // Do intial damage to enemy with intialDmg
        enemyCol.GetComponent<EnemyAI>().Hit(CalculateDamage(ProjectileElementalState, enemyElementalState, initialDmg));
        // Apply dot over time, till dotTime is elapsed
        for (int i = 0; i < dotTime; i++)
        {
            yield return new WaitForSeconds(1f);
            if (enemyCol == null)
                break;
            // Do damage to enemy
            enemyCol.GetComponent<EnemyAI>().Hit(dot);
        }
        Destroy(gameObject);
    }

    // Does knockback damage to hit enemy
    public virtual void KnockbackDamageToEnemy(float damage, float knockbackForce, Transform projTrans, Collider enemyCol)
    {
        ElementalStates enemyElementalState = enemyCol.GetComponent<EnemyAI>().Element;

        enemyCol.GetComponent<EnemyAI>().Hit(CalculateDamage(ProjectileElementalState, enemyElementalState, damage));
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

    private float CalculateDamage(ElementalStates ProjectileElementalState, ElementalStates otherElementalState, float dmg)
    {
        float damage = dmg;
        if ((ProjectileElementalState == ElementalStates.Fire && otherElementalState == ElementalStates.Earth) ||
            (ProjectileElementalState == ElementalStates.Earth && otherElementalState == ElementalStates.Wind) ||
            (ProjectileElementalState == ElementalStates.Wind && otherElementalState == ElementalStates.Water) ||
            (ProjectileElementalState == ElementalStates.Water && otherElementalState == ElementalStates.Fire))
        {
            // Apply damage multiplier if projectile elemental state is strong against enemy's elemental state
            damage *= AbilityManager.PlayerElementalDmgMultiplier;
        }
        else if ((ProjectileElementalState == ElementalStates.Fire && otherElementalState == ElementalStates.Water) ||
                 (ProjectileElementalState == ElementalStates.Water && otherElementalState == ElementalStates.Wind) ||
                 (ProjectileElementalState == ElementalStates.Wind && otherElementalState == ElementalStates.Earth) ||
                 (ProjectileElementalState == ElementalStates.Earth && otherElementalState == ElementalStates.Fire))   
        {
            // Reduce damage of projectile if projectie elemental state is weak to enemy's elemental state
            damage *= AbilityManager.ElementalEnemyDmgReductionMultiplier;
        }
        return damage;
    }
}
