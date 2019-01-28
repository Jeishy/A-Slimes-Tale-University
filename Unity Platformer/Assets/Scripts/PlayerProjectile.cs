using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour {
    [SerializeField] private Rigidbody proj;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private Transform projectileSpawnTrans;
    [SerializeField] private float fireRate;

    private Transform playerTrans;
    private float fireTime;

	// Use this for initialization
	private void Start () 
    {
        playerTrans = GetComponent<Transform>();
        fireTime = 0f;
	}
	
	// Update is called once per frame
	private void Update ()
    { 
        // Check if left mouse button is clicked
		if (Input.GetMouseButton(0))
        {
            Debug.Log("Left mouse clicked");
            FireProjectile();
        }
	}

    private void FireProjectile()
    {
        // Check if elapsed time is greater than fire time
        // (Used for cooldown)
        if (Time.time > fireTime)
        {
            // Set fire time variable to the fire rate + current time elapsed
            // ensures projectile only fired when new fire time is elapsed
            fireTime = fireRate + Time.time;
            // Instatiate projectie prefab from position of players front
            Rigidbody projectile = Instantiate(proj, projectileSpawnTrans.position, Quaternion.identity);

            // Get direction between mouse and player
            Vector3 mousePos = Input.mousePosition;
            Vector3 playerPos = Camera.main.WorldToScreenPoint(playerTrans.position);
            Vector3 direction = Vector3.Normalize(mousePos - playerPos);

            // Change velocity of projectile to calculated normalized direction vector * specified speed (magnitude)
            projectile.velocity = direction * projectileSpeed;
            Debug.Log("Velocity of projectile: " + projectile.velocity);
        }
        else
        {
            // Optional: display UI element alerting player that ability is on cooldown
            Debug.LogWarning("Projectile fire on cooldown!");
        }
    }
}
