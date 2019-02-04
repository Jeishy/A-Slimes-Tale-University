using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FireProjectile : ElementalProjectiles,IPooledProjectile {
	[SerializeField] private AbilityManager abilityManager;
	private Rigidbody rb;
	private Plane plane;
	[SerializeField] private Transform playerTrans;
	private Vector3 distanceFromCamera;
	
	public void Shoot()
	{
		DamageType = DamageTypes.DOT;
		rb = GetComponent<Rigidbody>();
		playerTrans = GameObject.Find("Player").GetComponent<Transform>();
		abilityManager = GameObject.Find("AbilityManager").GetComponent<AbilityManager>();
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
