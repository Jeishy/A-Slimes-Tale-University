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
	[SerializeField] private LayerMask m_WallLayer;                             // Layer of the walls
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, 2f)] [SerializeField] private float m_GroundMovementSmoothing = .05f;	// How much to smooth out the movement
	[Range(0, 2f)] [SerializeField] private float m_AirMovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	public LayerMask m_WhatIsGround;							                // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
	[SerializeField] private Transform m_WallCheck;								// Transform of a wall check object
    [SerializeField] private float m_wallDetectRadius = 0.1f;					// Radius of a sphere cast
	[SerializeField] private Collider m_CrouchDisableCollider;				    // A collider that will be disabled when crouching
	[SerializeField] private Vector2 m_KnockbackForce;							// Force applied when hit by an enemy
	[SerializeField] private float m_KnockbackLength;							// Period of time the player will not be able to move after getting knocked bakck
    [SerializeField] private GameObject m_WallJumpEffect;                       // Jumping particle effect
	

	const float k_GroundedRadius = .2f;                                         // Radius of the overlap circle to determine if grounded
	[HideInInspector] public bool m_Grounded;                                   // Whether or not the player is grounded.
	private bool m_wallSliding;			                                        // Whether or not the player is touching the wall
	const float k_CeilingRadius = .2f;                                          // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody m_Rigidbody;                                              // Rigidbody of the player
	private bool m_FacingRight = true;                                          // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;                                  // Velocity variable used to calculate movement
	private float m_WallNormal;                                                 // Stores the normal of the last touched wall
	private bool m_WallJumped = false;                                          // Is set to true when player has jumped from a wall and is in air
	
	private float m_KnockbackCount;                                             // Cooldown timer for the knockback effect when hit
	private bool m_KnockbackRight;		                                        // Used to determine direction when applying knockback force
    private Vector3 m_TouchedWallPoint;                                         // Position of the point on the last touched wall
    private float m_FlipDelay;                                                  // Flip delay counter
	private AudioManager _audioManager;                                         // Audio Manager reference
    
	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

	private void Awake()
	{
		m_Rigidbody = GetComponent<Rigidbody>();                                // Gets the rigidbody component from the player
		_audioManager = AudioManager.instance;                                  // Gets the instance of the AudioManager

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
		
	}

    //Fixed update is used when working with rigidbodies and forces
	private void FixedUpdate()
	{

        //Flag variables 
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

    //Called from the PlayerControls.cs class where input is retrieved 
	public void Move(float move, bool jump)
	{

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

            
			Vector3 targetVelocity;
            //If knockback effect has not been applied
            if (m_KnockbackCount <= 0)
			{
				// Move the character by finding the target velocity
				targetVelocity = new Vector2(move * 10f, m_Rigidbody.velocity.y);
			}
            //Else apply the knockback effect
			else
			{
                //To the right
				if (m_KnockbackRight)
				{
					targetVelocity = new Vector2(-m_KnockbackForce.x, m_KnockbackForce.y);
				}
                //To the left
				else
				{
					targetVelocity = new Vector2(m_KnockbackForce.x, m_KnockbackForce.y);
				}

                //Decrease knockback over time
				m_KnockbackCount -= Time.deltaTime;
			}

			// Gruond movement smoothing
			if (m_Grounded) 
				m_Rigidbody.velocity = Vector3.SmoothDamp(m_Rigidbody.velocity, targetVelocity, ref m_Velocity, m_GroundMovementSmoothing);


            // Air movement smoothing
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

        // Limit player wall slide speed to value of 'm_MaxWallSlideSpeed'
        if (m_wallSliding && m_Rigidbody.velocity.y <= -m_MaxWallSlideSpeed)
		{
			m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, -m_MaxWallSlideSpeed);

		}


        //Unstick from wall whenever player tries to move away from the wall
        if (GetRoundedInputs() == m_WallNormal)
        {
            StickToWall(false);
            //Apply small force away from the wall
            m_Rigidbody.AddForce(new Vector3(m_WallNormal * 10, 0, 0));
        }


        // If the player should jump...
        if (((m_Grounded || m_wallSliding) && jump))
		{
			
			StickToWall(false);
            m_Grounded = false;

            //Play jump sound
			_audioManager.Play("PlayerJump");

			// ... and player is touching the wall
			if (m_wallSliding && !m_Grounded)
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

    //Sets flags for the knockback system above
	public void Knockback(bool right)
	{
		m_KnockbackRight = right;
		m_KnockbackCount = m_KnockbackLength;
	}


    //Returns -1 if moving left, 0 if stationary and 1 if moving right
    private int GetRoundedInputs() {
        int input = 0;
        if (Input.GetAxis("Horizontal") < 0)
            input = -1;
        else if (Input.GetAxis("Horizontal") > 0)
            input = 1;

        return input;
    }


    //Flips the character by 180 degrees and also flips the scale of the object
    private void Flip()
	{
        //Small delayed required between flips, otherwise bugs may occur
        if (m_FlipDelay <= Time.time) 
        {
            //Make sure the player doesnt flip when is already in the good position whilst wall sliding
            if (m_wallSliding && (m_FacingRight && m_WallNormal == 1) || (!m_FacingRight && m_WallNormal == -1))
                return;

            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            m_FlipDelay = 0.1f + Time.time;
        }
	}
 
    //This is called whenever the player collides with a wall
    private void StickToWall(bool stick)
    {

        //Freezes rigidbodies constraints to prevent moving horizontally whilst stuck on the wall
        if (stick)
            m_Rigidbody.constraints =     RigidbodyConstraints.FreezePositionX |
                                            RigidbodyConstraints.FreezeRotation;
        //Unstick from the wall
        else
        {
            m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

	private void OnCollisionEnter(Collision other)
	{
        //If the player collided with the wall...
		if (other.gameObject.CompareTag("Wall"))
		{
            //Store the wall normal
			m_WallNormal = other.contacts[0].normal.x;
            
	         //Possible alternative fix for poor wall jumping
            if (Mathf.Abs(m_WallNormal) == 1)
            {
                //Set wall sliding flag to true and stick to wall
                m_wallSliding = true;
                StickToWall(true);
            }
		}
	}

    private void OnCollisionStay(Collision other)
    {
        m_TouchedWallPoint = other.contacts[0].point;       // Getting the point last touched for the jump particle effect
    }

	private void OnCollisionExit(Collision other)
	{
        //Unstick from wall
        m_wallSliding = false;
        StickToWall(false);
    }
}