using System.Collections;
using UnityEngine;

[System.Serializable]
public class EnemyAttack {

    public AttackStyle attackStyle;
    public float meleeRange = 0.5f;
    public float range = 5f;
    public float attackSpeed = 1f;
    public float animationDamageDelay = 0.1f;
    public float ghostMoveAttackSpeed;
    public GameObject projectile;
    public Transform firePoint;
    public GameObject particleEffect;
    public float projectileSpeed = 10f;
    public float projectileLifespan = 3f;
    public float particleLifespan = 3f;
    public LayerMask playerMask;
    public LayerMask rangeAttackMask;
    public bool rotate;
    public Transform rotator;
    public float rotateSpeed = 1f;


}

public enum AttackStyle
{
    Melee, Ranged, Ghost
}
