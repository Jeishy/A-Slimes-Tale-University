using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour {
    [SerializeField] private bool isAimToShoot;
    [SerializeField] [Tooltip("Ensure Projetile Speed is set relative to speed of player")] private float projectileSpeed;
    [SerializeField] private Transform projectileSpawnTrans;
    [SerializeField] private float fireRate;

    private Transform playerTrans;
    private float fireTime;
    private ProjectilePooler projPooler;

	// Use this for initialization
	private void Start () 
    {
        playerTrans = GetComponent<Transform>();
        projPooler = GetComponent<ProjectilePooler>();
        fireTime = 0f;
	}
	
	// Update is called once per frame
	private void Update ()
    {
        // Toggles aiming mode from aimed to forward
        // Note: Ensure button mapping is set for this action
        // instead of Input.GetKeyDown
        if (Input.GetKeyDown(KeyCode.E))
        {
            isAimToShoot = !isAimToShoot;
        }

        // Check if mouse button 0 (Left click) is clicked and 
        // if elapsed time is greater than fire time (Used for cooldown)
        if (Input.GetMouseButton(0) && Time.time > fireTime)
        {
            // Set fire time variable to the fire rate + current time elapsed
            // ensures projectile only fired when new fire time is elapsed
            fireTime = fireRate + Time.time;
            // Instatiate projectie prefab from position of players front
            GameObject projectile = projPooler.SpawnProjectileFromPool("Projectiles", projectileSpawnTrans.position, Quaternion.identity);
            Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();
            if (isAimToShoot)
            {
                Debug.Log("Aimed mode");
                AimToFireProjectile(projectileRB);
            }
            else
            {
                Debug.Log("Forward fire mode");
                FireProjectileForward(projectileRB);
            }
        }
        else
        {
            // Optional: display UI element alerting player that ability is on cooldown
            Debug.LogWarning("Projectile fire on cooldown!");
        }
	}

    private void AimToFireProjectile(Rigidbody projectile)
    {
        // Get direction between mouse and player
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerPos = Camera.main.WorldToScreenPoint(playerTrans.position);
        Vector3 direction = Vector3.Normalize(mousePos - playerPos);

        // Change velocity of projectile to calculated normalized direction vector * specified speed (magnitude)
        projectile.velocity = direction * projectileSpeed;
        Debug.Log("Velocity of projectile: " + projectile.velocity);
    }

    private void FireProjectileForward(Rigidbody projectile)
    {
        // Get transform's forward direction
        Vector3 forwardDirection = transform.forward;

        // Change velocity of projectile to forward direction vector * specified speed (magnitude)
        projectile.velocity = forwardDirection * projectileSpeed;
        Debug.Log("Velocity of projectile: " + projectile.velocity);
    }
}
