using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FireProjectile : ElementalProjectiles,IPooledProjectile {

	private Vector3 originalScale;
	private Rigidbody2D rb;
	private Plane plane;							// Plane used for plane.raycast in the Shoot function
	private Vector3 distanceFromCamera;				// Distance from camera that the plane is created at
	[SerializeField] private float dot;				// DOT for fire projectile
	[SerializeField] private float boostedDot;		// Boosted DOT for fire projectile
	[SerializeField] private int dotTime;			// Time over which dot is applied. Note: This is in seconds
	[SerializeField] private int boostedDotTime;	// Time over which boosted dot is applied

	// Rigidbody is set in awake
	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		originalScale = transform.localScale;
	}

	public void Shoot()
	{
		// Null check to ensure player variables are set
		if (playerTrans == null)
			LoadPlayerVariables();

		StartCoroutine(GravityDropOff(rb));

		distanceFromCamera = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, playerTrans.position.z);
		plane = new Plane(Vector3.forward, distanceFromCamera);

		if (abilityManager.IsAimToShoot)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			float enter = 1000.0f;
			if (plane.Raycast(ray, out enter))
			{
				// Set gameobjects velocity to returned value of AimToFireProjectileForce method
				rb.velocity = AimToFireProjectileForce(ProjectileSpeed, ray, enter, playerTrans);
				// Debug ray to see raycast in viewport
				Debug.DrawRay(ray.origin, ray.direction * enter, Color.green, 2f);
			}
		}
		else
		{
			// Sets go's velocity if forward firing projectiles are chosen
    		rb.velocity = FireProjectileForward(ProjectileSpeed, playerTrans);
		}
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Enemy"))
		{
			// Apply dot to enemy with params
			if (IsBoosted)
				StartCoroutine(DOTToEnemy(boostedDot, boostedDotTime, gameObject, col));
			else
				StartCoroutine(DOTToEnemy(dot, dotTime, gameObject, col));
		}
		// Deactive projectile and display particle effect
		// Set projectile back to normal once it hits something
		if (IsBoosted)
		{
			Debug.Log(originalScale);
			transform.localScale = originalScale;
			IsBoosted = false;
		}
		rb.gravityScale = 0;
		gameObject.SetActive(false);
	}
}
