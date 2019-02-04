using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageTypes
{
	DOT, Knockback, FlatDamage
}

// Creating asset menu so that SOs can be created in editor
public class ElementalProjectiles : MonoBehaviour {

    // Note: ACCESS ALL VARIABLES VIA GET AND SET ACCESSORS!
	// public AudioClip projectileSound; uncomment when sounds are added
    [HideInInspector] public bool IsBoosted;
    [HideInInspector] public DamageTypes DamageType;
	public float ProjectileSpeed;
	public float BoostedDamage;
    private Vector3 projForce;
    private Vector3 hitPoint;

    public virtual Vector3 AimToFireProjectileForce(float projectileSpeed, Ray ray, float enter, Transform playerTrans)
    {
        hitPoint = ray.GetPoint(enter);
        // Get direction between mouse and player
        Vector3 playerPos = playerTrans.position;
        hitPoint = new Vector3(hitPoint.x, hitPoint.y, playerPos.z);
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

    public virtual void Damage(float damage, DamageTypes damageType)
    {
        // Do normal damage to enemy here
    }

    public virtual void Damage(float boostedDamage, DamageTypes damageType, bool isBoosted)
    {
        // Do boosted damage to enemy here
    }
}
