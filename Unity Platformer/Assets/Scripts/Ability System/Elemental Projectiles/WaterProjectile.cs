using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WaterProjectile : ElementalProjectiles,IPooledProjectile {
	
	private Vector3 _originalScale;
	private Rigidbody2D _rb;
	private Plane _plane;				// Plane used for plane.raycast in the Shoot function
	private Vector3 _distanceFromCamera;	// Distance from camera that the plane is created at
	[SerializeField] private float _baseDamage;
	[SerializeField] private float _boostedDamage;
	
	// Rigidbody is set in awake
	private void Awake()
	{
		_rb = GetComponent<Rigidbody2D>();
		_originalScale = transform.localScale;
	}	

	public void Shoot()
	{
		// Null check to ensure player variables are set
		if (playerTrans == null)
			LoadPlayerVariables();

		StartCoroutine(GravityDropOff(_rb));

        // Destroy the projectile after specified number of seconds
        Destroy(gameObject, TimeTillDestroy);

        _distanceFromCamera = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, playerTrans.position.z);
		_plane = new Plane(Vector3.forward, _distanceFromCamera);

		if (abilityManager.IsAimToShoot)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			float enter = 1000.0f;
			if (_plane.Raycast(ray, out enter))
			{
				// Set gameobjects velocity to returned value of AimToFireProjectileForce method
				_rb.velocity = AimToFireProjectileForce(ProjectileSpeed, ray, enter, playerTrans);
				// Debug ray to see raycast in viewport
				Debug.DrawRay(ray.origin, ray.direction * enter, Color.green, 2f);
			}
		}
		else
		{
			// Sets go's velocity if forward firing projectiles are chosen
    		_rb.velocity = FireProjectileForward(ProjectileSpeed, playerTrans);
		}
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Enemy"))
		{
			if (IsBoosted)
				FlatDamageToEnemy(_boostedDamage, col);
			else
				FlatDamageToEnemy(_baseDamage, col);
		}
		// Deactive projecitle and display a particle effect
		// Set projectile back to normal once it hits something
		if (IsBoosted)
		{
			transform.localScale = _originalScale;
			IsBoosted = false;
		}
		_rb.gravityScale = 0;
		Destroy(gameObject);
	}
}
