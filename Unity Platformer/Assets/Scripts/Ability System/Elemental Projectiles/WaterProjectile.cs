using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WaterProjectile : ElementalProjectiles, IPooledProjectile 
{
	
	private Vector3 _originalScale;
	private Rigidbody _rb;
	private Plane _plane;				// Plane used for plane.raycast in the Shoot function
	private Vector3 _distanceFromCamera;    // Distance from camera that the plane is created at
    private AbilityInputHandler _inputHandler;

    [SerializeField] private float _baseDamage;
	[SerializeField] private float _boostedDamage;
    [SerializeField] private GameObject _OnLandPE;
	
	// Rigidbody is set in awake
	private void Awake()
	{
        projectileElementalState = ElementalStates.Water;
		_rb = GetComponent<Rigidbody>();
		_originalScale = transform.localScale;
        _inputHandler = GameObject.FindGameObjectWithTag("AbilityManager").GetComponent<AbilityInputHandler>();
    }

    public void Shoot()
	{
        // Set gravity to false;
        _rb.useGravity = false;

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
            Vector2 joystickDir = _inputHandler.RightStickAxis;
            _rb.velocity = JoystickFiringForce(ProjectileSpeed, playerTrans, joystickDir);
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        GameObject onLand = Instantiate(_OnLandPE, collision.contacts[0].point, Quaternion.identity);
        if (IsBoosted)
            onLand.transform.localScale *= 2.0f;
        Destroy(onLand, 1f);
        Collider col = collision.collider;
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
        _rb.useGravity = false;
        Destroy(gameObject);
    }
}
