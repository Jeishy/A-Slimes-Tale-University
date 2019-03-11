using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0.25f, 1)] [SerializeField] private float m_WallJumpVerticalForceMultiplier = 0.3f;	// Amount the jump force is multiplied by when jumping off a wall
	[SerializeField] private float m_HorizontalJumpForce = 200f;				// Amount of lateral force added when the player jumps from a wall
	[SerializeField] private float m_MaxWallSlideSpeed;							// Maximum wall sliding speed
	[SerializeField] private LayerMask m_WallLayer;
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
    [SerializeField] private GameObject m_WallJumpEffect;
	

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	[HideInInspector] public bool m_Grounded;            // Whether or not the player is grounded.
	private bool m_wallSliding;			// Whether or not the player is touching the wall
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
	private float m_WallNormal;
	private bool m_WallJumped = false;
	
	private bool m_PhysicsWallCheck;
	private float m_KnockbackCount;
	private bool m_KnockbackRight;		// Used to determine direction when applying knockback force
    private Vector3 m_TouchedWallPoint;
    
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
					targetVelocity = new Vector2(-m_KnockbackForce.x, m_KnockbackForce.y);
				}
				else
				{
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
		Vector2 facingDirection = new Vector2(Input.GetAxis("Horizontal"), 0f);
		
		//m_wallSliding = Physics2D.Raycast(transform.position, facingDirection, m_wallDetectRadius, m_WallLayer);
		m_wallSliding = Physics2D.OverlapCircle(m_WallCheck.position, m_wallDetectRadius, m_WallLayer);

		
		// Limit player wall slide speed
		if (m_wallSliding && m_Rigidbody2D.velocity.y <= -m_MaxWallSlideSpeed)
		{
			
			m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, -m_MaxWallSlideSpeed);

		}

        if (m_WallNormal == Input.GetAxis("Horizontal") && m_WallNormal != 0)
        {
            Debug.Log("Unsticking from wall");
            StickToWall(false);
        }


        // If the player should jump...
        if (((m_Grounded || m_wallSliding) && jump))
		{
			
			StickToWall(false);
            m_Grounded = false;

			// ... and player is touching the wall
			if (m_wallSliding)
			{
				// Add a vertical and horizontal force to the player if he is sliding on the wall
				m_Rigidbody2D.AddForce(new Vector2((m_HorizontalJumpForce * m_WallNormal) , (m_JumpForce * m_WallJumpVerticalForceMultiplier)));
				m_WallJumped = true;
                // Display wall jumping particle effect
                GameObject pWallJump = Instantiate(m_WallJumpEffect, m_TouchedWallPoint, Quaternion.identity);
                Destroy(pWallJump, 1f);
				// Flip because character will jump off the wall in the other direction
				Flip();
				Debug.Log("Wall jump, Wall normal: " + m_WallNormal);
			}
			
			// ... and player is not touching the wall
			else
			{
				Debug.Log("Normal jump");
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
 
    private void StickToWall(bool stick)
    {
        if (stick)
            m_Rigidbody2D.constraints =     RigidbodyConstraints2D.FreezePositionX |
                                            RigidbodyConstraints2D.FreezeRotation;
        else
        {
            m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Wall"))
		{
			
			
			m_WallNormal = other.contacts[0].normal.x;


            
	         //Possible alternative fix for poor wall jumping
            if (Mathf.Abs(m_WallNormal) == 1)
            {
                StickToWall(true);
            }
		}
	}

    private void OnCollisionStay2D(Collision2D other)
    {
        m_TouchedWallPoint = other.contacts[0].point;       // Getting the point last touched 
    }

    // Possible alternative fix for poor wall jumping
	private void OnCollisionExit2D(Collision2D other)
	{
        StickToWall(false);
    }
}