using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour {
    [SerializeField] private Rigidbody proj;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private Transform projectileSpawn;
    [SerializeField] private float fireRate;

    private Transform playerTrans;
    private float fireTime;

	// Use this for initialization
	void Start () {
        playerTrans = GetComponent<Transform>();
        fireTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
    { 
		if (Input.GetMouseButton(0))
        {
            FireProjectile();
        }
	}

    private void FireProjectile()
    {
        if (Time.time > fireTime)
        {
            fireTime = fireRate + Time.time;
            // Instatiate projectie prefab from position of players front
            Rigidbody projectile = Instantiate(proj, projectileSpawn.position, Quaternion.identity);

            // Get direction between mouse and player
            Vector3 mousePos = Input.mousePosition;
            Vector3 playerPos = Camera.main.WorldToScreenPoint(playerTrans.position);
            Vector3 direction = Vector3.Normalize(mousePos - playerPos);

            // Apply force to proj in calculated direction
            projectile.velocity = direction * projectileSpeed;
            Debug.Log("Velocity of projectile: " + projectile.velocity);
        }
        else
        {
            Debug.LogWarning("Projectile fire on cooldown!");
        }
    }
}
