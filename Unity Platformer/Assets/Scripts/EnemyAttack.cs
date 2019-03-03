using System.Collections;
using UnityEngine;

[System.Serializable]
public class EnemyAttack {

    public bool melee = true;
    public float meleeRange = 0.5f;
    public float range = 5f;
    public float attackSpeed = 1f;
    public GameObject projectile;
    public float projectileSpeed = 10f;
    public float projectileLifespan = 3f;
    public LayerMask playerMask;
    public LayerMask rangeAttackMask;


}
