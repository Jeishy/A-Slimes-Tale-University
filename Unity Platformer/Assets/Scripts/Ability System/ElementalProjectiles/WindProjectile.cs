using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WindProjectile : ElementalProjectiles,IPooledProjectile {
	private Rigidbody2D rb;
	private Plane plane;				// Plane used for plane.raycast in the Shoot function
	private Vector3 distanceFromCamera;	// Distance from camera that the plane is created at
	[SerializeField] private float knockbackForce;
	[SerializeField] private float boostedKnockbackForce;
	[SerializeField] private float baseDamage;
	[SerializeField] private float boostedDamage;
	
	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
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
			Debug.Log("Aimed firing");
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
			if (IsBoosted)
				KnockbackDamageToEnemy(boostedDamage, boostedKnockbackForce, transform, col);
			else
				KnockbackDamageToEnemy(baseDamage, knockbackForce, transform, col);
			// Add hit effects here
		}
		// Deactive projectile and display particle effect
		gameObject.SetActive(false);
	}
}
