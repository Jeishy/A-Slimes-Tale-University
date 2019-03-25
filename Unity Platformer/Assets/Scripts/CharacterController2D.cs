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
    [SerializeField] private Transform m_BehindWallCheck;
    [SerializeField] private float m_wallDetectRadius = 0.1f;					// Radius of a sphere cast
	[SerializeField] private Collider m_CrouchDisableCollider;				// A collider that will be disabled when crouching
	[SerializeField] private Vector2 m_KnockbackForce;							// Force applied when hit by an enemy
	[SerializeField] private float m_KnockbackLength;							// Period of time the player will not be able to move after getting knocked bakck
    [SerializeField] private GameObject m_WallJumpEffect;
	

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	[HideInInspector] public bool m_Grounded;            // Whether or not the player is grounded.
	private bool m_wallSliding;			// Whether or not the player is touching the wall
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody m_Rigidbody;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
	private float m_WallNormal;
	private bool m_WallJumped = false;
	
	private bool m_PhysicsWallCheck;
	private float m_KnockbackCount;
	private bool m_KnockbackRight;		// Used to determine direction when applying knockback force
    private Vector3 m_TouchedWallPoint;
    private float m_FlipDelay;
    
	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

	private void Awake()
	{
		m_Rigidbody = GetComponent<Rigidbody>();

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
		Collider[] colliders = Physics.OverlapSphere(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
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


	public void Move(float move, bool jump)
	{
		
		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			Vector3 targetVelocity;
			if (m_KnockbackCount <= 0)
			{
				// Move the character by finding the target velocity
				targetVelocity = new Vector2(move * 10f, m_Rigidbody.velocity.y);
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
				m_Rigidbody.velocity = Vector3.SmoothDamp(m_Rigidbody.velocity, targetVelocity, ref m_Velocity, m_GroundMovementSmoothing);


			if (!m_Grounded)
			{
				m_Rigidbody.velocity = Vector3.SmoothDamp(m_Rigidbody.velocity, targetVelocity, ref m_Velocity, m_AirMovementSmoothing);
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

        // Limit player wall slide speed
        if (m_wallSliding && m_Rigidbody.velocity.y <= -m_MaxWallSlideSpeed)
		{
            Debug.Log("LIMITING WALL SPEED!!!");
			m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, -m_MaxWallSlideSpeed);

		}


        //Unstick from wall whenever player tries to move away from the wall
        if (m_WallNormal == Input.GetAxis("Horizontal") && m_WallNormal != 0)
        {
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
				m_Rigidbody.AddForce(new Vector2((m_HorizontalJumpForce * m_WallNormal) , (m_JumpForce * m_WallJumpVerticalForceMultiplier)));
				m_WallJumped = true;
                // Display wall jumping particle effect
                GameObject pWallJump = Instantiate(m_WallJumpEffect, m_TouchedWallPoint, Quaternion.identity);
                Destroy(pWallJump, 1f);
				// Flip because character will jump off the wall in the other direction
				Flip();
			}
			
			// ... and player is not touching the wall
			else
			{
				// Add only the vertical force to the player.
				m_Rigidbody.AddForce(new Vector2(0f, m_JumpForce), ForceMode.Force);
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
        if (m_FlipDelay <= Time.time)
            
        {

            if (m_wallSliding && (m_FacingRight && m_WallNormal == 1) || (!m_FacingRight && m_WallNormal == -1))
                return;

            //Debug.Log("flipping");
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            m_FlipDelay = 0.1f + Time.time;
        }
	}
 
    private void StickToWall(bool stick)
    {
        if (stick)
            m_Rigidbody.constraints =     RigidbodyConstraints.FreezePositionX |
                                            RigidbodyConstraints.FreezeRotation;
        else
        {
            m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Wall"))
		{
			m_WallNormal = other.contacts[0].normal.x;
            
	         //Possible alternative fix for poor wall jumping
            if (Mathf.Abs(m_WallNormal) == 1)
            {
                m_wallSliding = true;
                StickToWall(true);
            }
		}
	}

    private void OnCollisionStay(Collision other)
    {
        m_TouchedWallPoint = other.contacts[0].point;       // Getting the point last touched 
    }

    // Possible alternative fix for poor wall jumping
	private void OnCollisionExit(Collision other)
	{
        m_wallSliding = false;
        StickToWall(false);
    }
}