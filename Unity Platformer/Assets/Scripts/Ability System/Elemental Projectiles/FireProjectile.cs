using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FireProjectile : ElementalProjectiles {

	private Vector3 _originalScale;
	private Rigidbody _rb;
	private Plane _plane;							// Plane used for plane.raycast in the Shoot function
	private Vector3 _distanceFromCamera;		    // Distance from camera that the plane is created at
    private AbilityInputHandler _inputHandler;
    private bool _hasHit;

    [SerializeField] private float _initialDmg;  // Initial damage done by projectile
	[SerializeField] private float _dot;			// DOT for fire projectile
	[SerializeField] private float _boostedDot;		// Boosted DOT for fire projectile
	[SerializeField] private int _dotTime;			// Time over which dot is applied. Note: This is in seconds
	[SerializeField] private int _boostedDotTime;	// Time over which boosted dot is applied
    [SerializeField] private GameObject _onLandPE;
    [SerializeField] private GameObject[] _projPE = new GameObject[3];

	// Rigidbody is set in awake
	private void Awake()
	{
        ProjectileElementalState = ElementalStates.Fire;
		_rb = GetComponent<Rigidbody>();
		_originalScale = transform.localScale;
        _inputHandler = GameObject.FindGameObjectWithTag("AbilityManager").GetComponent<AbilityInputHandler>();
        _hasHit = false;
    }

	public void Shoot()
    {
        // Set gravity to false;
        _rb.useGravity = false;

        // Null check to ensure player variables are set
        if (PlayerTrans == null)
			LoadPlayerVariables();

        // Destroy the projectile after specified number of seconds
        Destroy(gameObject, TimeTillDestroy);

		_distanceFromCamera = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, PlayerTrans.position.z);
		_plane = new Plane(Vector3.forward, _distanceFromCamera);

		if (AbilityManager.IsAimToShoot)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			float enter = 1000.0f;
			if (_plane.Raycast(ray, out enter))
			{
				// Set gameobjects velocity to returned value of AimToFireProjectileForce method
				_rb.velocity = AimToFireProjectileForce(ProjectileSpeed, ray, enter, PlayerTrans);
				// Debug ray to see raycast in viewport
				Debug.DrawRay(ray.origin, ray.direction * enter, Color.green, 2f);
			}
		}
		else
		{
            // Sets go's velocity if forward firing projectiles are chosen
            Vector2 joystickDir = _inputHandler.RightStickAxis;
    		_rb.velocity = JoystickFiringForce(ProjectileSpeed, PlayerTrans, joystickDir);
		}
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (!_hasHit)
        {
            _hasHit = true;
            GameObject onLand = Instantiate(_onLandPE, collision.contacts[0].point, Quaternion.identity);
            if (IsBoosted)
                onLand.transform.localScale *= 2.0f;
            Destroy(onLand, 1f);
        }

        Collider col = collision.collider;
        if (col.CompareTag("Enemy"))
        {
            StopAllCoroutines();
            // Apply dot to enemy depending on if projectile is boosted or not
            StartCoroutine(IsBoosted
                ? DotToEnemy(_initialDmg, _boostedDot, _boostedDotTime, gameObject, col)
                : DotToEnemy(_initialDmg, _dot, _dotTime, gameObject, col));

        }
        // Hide projectile till its destroyed
        GetComponent<MeshRenderer>().enabled = false;
        for (int i = 0; i < _projPE.Length; i++)
        {
            _projPE[i].SetActive(false);
        }
    }
}
