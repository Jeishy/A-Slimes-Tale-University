using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private bool canMove = true;
	
	// Use this for initialization
	void Start ()
	{
        player = GetComponent<Player>();
		controller = GetComponent<CharacterController2D>();
    }
    
	// Update is called once per frame
	void Update ()
	{
        if (canMove)
        {
            Movement();
        }
        
    }

    private void Movement()
    {
        if (!player.isDead)
        {
            horizontalMove = Input.GetAxis("Horizontal") * speed;
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
        }
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void EnableMovement()
    {
        if (!canMove)
            canMove = true;
    }

    public void DisableMovement()
    {
        if (canMove)
            canMove = false;
    }
}
