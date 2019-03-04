using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerControls : MonoBehaviour
{


	[SerializeField] private float speed = 40f;
    [SerializeField] private GameObject normalJumpParticles;
    [SerializeField] private ParticleSystem slimeTrail;
    [SerializeField] private Transform grounCheckTrans;


    private CharacterController2D controller;
    private Player player;
	private float horizontalMove;
	private bool jump = false;
	private bool wallJump = true;
	private bool crouch = false;
	
	// Use this for initialization
	void Start ()
	{
        player = GetComponent<Player>();
		controller = GetComponent<CharacterController2D>();
    }
    
	
	// Update is called once per frame
	void Update ()
	{

        if (!player.isDead)
        {

            //Gets button presses for sideways movement
            horizontalMove = Input.GetAxisRaw("Horizontal") * speed;
            if (!controller.m_Grounded)
                slimeTrail.Stop();
            else
                slimeTrail.Play();


            //Checks if player has pressed the jump button
            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
                if (controller.m_Grounded)
                {
                    // Spawn particle effect at base of player
                    GameObject jumpParticles = Instantiate(normalJumpParticles, grounCheckTrans.position, Quaternion.Euler(-90, 0, 0));
                    Destroy(jumpParticles, 1f);
                }
            }


            //Crouching, saved for future
            /*if (Input.GetButtonDown("Crouch"))
            {
                Debug.Log("Crouch down");
                crouch = true;
            }

            else if (Input.GetButtonUp("Crouch"))
            {
                Debug.Log("Crouch up");
                crouch = false;
            }*/


        }

	}

	void FixedUpdate()
	{
		controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
		jump = false;
	}
}
