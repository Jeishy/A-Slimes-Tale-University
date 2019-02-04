using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EarthProjectile : ElementalProjectiles,IPooledProjectile {
	private Rigidbody rb;
	private Plane plane;
	private Vector3 distanceFromCamera;
	
	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	public void Shoot()
	{
		if (playerTrans == null)
			LoadPlayerVariables();
			
		// Note: Damage type is subject to change
		DamageType = DamageTypes.FlatDamage;
		distanceFromCamera = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, playerTrans.position.z);
		plane = new Plane(Vector3.forward, distanceFromCamera);

		if (abilityManager.IsAimToShoot)
		{
			Debug.Log("Aimed firing");
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			float enter = 1000.0f;
			if (plane.Raycast(ray, out enter))
			{
				rb.velocity = AimToFireProjectileForce(ProjectileSpeed, ray, enter, playerTrans);
				Debug.DrawRay(ray.origin, ray.direction * enter, Color.green, 2f);
			}
		}
		else
		{
    		rb.velocity = FireProjectileForward(ProjectileSpeed, playerTrans);
		}
	}
}
