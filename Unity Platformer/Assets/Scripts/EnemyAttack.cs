using System.Collections;
using UnityEngine;

[System.Serializable]
public class EnemyAttack {

    public AttackStyle attackStyle;                                     // Enum with different enemy attack types
    public float meleeRange = 0.5f;                                     // Detection distance for melee enemy attacks
    public float range = 5f;                                            // Detection distance for range enemy attacks
    public float attackSpeed = 1f;                                      // Rate of attack
    public float animationDamageDelay = 0.1f;                           // Delay to synchronise damage and enemy attack animations
    public float ghostMoveAttackSpeed;                                  // Special ghost attack flying speed
    public GameObject projectile;                                       // Projectile game object (only ranged)
    public Transform firePoint;                                         // Projectile spawn transform (only ranged)
    public GameObject particleEffect;                                   // Attack particle effect
    public float projectileSpeed = 10f;                                 // Projectile velocity (only ranged)
    public float projectileLifespan = 3f;                               // Projectile gets deleted after this time (only ranged)
    public float particleLifespan = 3f;                                 // Attack particle effect lifetime
    public LayerMask playerMask;                                        // Player layer mask
    public LayerMask rangeAttackMask;                                   // Player and Wall layer mask (ensures enemies shoot with direct line of fire)
    public bool rotate;                                                 // If true the enemy will rotate its rotator towards the player (cannon rotates its barrel)
    public Transform rotator;                                           // Rotator transform
    public float rotateSpeed = 1f;                                      // Rotate speed


}

public enum AttackStyle
{
    Melee, Ranged, Ghost
}
