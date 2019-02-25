using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0.25f, 1)] [SerializeField] private float m_WallJumpVerticalForceMultiplier = 0.3f;	// Amount the jump force is multiplied by when jumping off a wall
	[SerializeField] private float m_HorizontalJumpForce = 200f;				// Amount of lateral force added when the player jumps from a wall
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, 2f)] [SerializeField] private float m_GroundMovementSmoothing = .05f;	// How much to smooth out the movement
	[Range(0, 2f)] [SerializeField] private float m_AirMovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
	[SerializeField] private Transform m_WallCheck;								// Transform of a wall check object
	[SerializeField] private float m_wallDetectRadius = 0.1f;					// Radius of a sphere cast
	[SerializeField] private Collider2D m_CrouchDisableCollider;				// A collider that will be disabled when crouching
	[SerializeField] private Vector2 m_KnockbackForce;							// Force applied when hit by an enemy
	[SerializeField] private float m_KnockbackLength;							// Period of time the player will not be able to move after getting knocked bakck
	

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	[HideInInspector] public bool m_Grounded;            // Whether or not the player is grounded.
	private bool m_wallSliding;			// Whether or not the player is touching the wall
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
	private float m_WallNormal;
	private bool m_WallJumped = false;
	private int m_LayerMask;
	private bool m_PhysicsWallCheck;
	private float m_KnockbackCount;
	private bool m_KnockbackRight;		// Used to determine direction when applying knockback force
	
	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
		
		m_LayerMask = LayerMask.GetMask("Wall");
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;
		m_WallJumped = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}	
	}


	public void Move(float move, bool crouch, bool jump)
	{
		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
			}
		}

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			// If crouching
			if (crouch)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			} else
			{
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}


			Vector3 targetVelocity;
			if (m_KnockbackCount <= 0)
			{
				// Move the character by finding the target velocity
				targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			}
			else
			{
				if (m_KnockbackRight)
				{
					Debug.Log("Right");
					targetVelocity = new Vector2(-m_KnockbackForce.x, m_KnockbackForce.y);
				}
				else
				{
					Debug.Log("left");
					targetVelocity = new Vector2(m_KnockbackForce.x, m_KnockbackForce.y);
				}

				m_KnockbackCount -= Time.deltaTime;
			}

			// And then smoothing it out and applying it to the character
			if (m_Grounded) 
				m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_GroundMovementSmoothing);


			if (!m_Grounded)
			{
				m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_AirMovementSmoothing);
			}
			
			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		
		// Wall Sliding
		m_wallSliding = Physics2D.OverlapCircle(m_WallCheck.position, m_wallDetectRadius, m_LayerMask);

		if (m_wallSliding && m_Rigidbody2D.velocity.y <= -0.7f)
		{
			
			m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, -0.7f);

		}
				
		// If the player should jump...
		if ((m_Grounded || m_wallSliding) && jump)
		{

			m_Grounded = false;

			// ... and player is touching the wall
			if (m_wallSliding)
			{
				// Add a vertical and horizontal force to the player if he is sliding on the wall
				m_Rigidbody2D.AddForce(new Vector2((m_HorizontalJumpForce * m_WallNormal) , (m_JumpForce * m_WallJumpVerticalForceMultiplier)));
				m_WallJumped = true;
				
				// Flip because character will jump off the wall in the other direction
				Flip();
			}
			
			// ... and player is not touching the wall
			else
			{
				// Add a vertical force to the player.
				m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce), ForceMode2D.Force);
			}
		}
	}

	public void Knockback(bool right)
	{
		m_KnockbackRight = right;
		m_KnockbackCount = m_KnockbackLength;
	}
	
	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
 
	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Wall"))
		{
			m_WallNormal = other.contacts[0].normal.x;
			
		}
	}
}