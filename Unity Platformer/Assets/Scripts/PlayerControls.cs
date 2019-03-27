using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PS4;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerControls : MonoBehaviour
{


	[SerializeField] private float speed = 40f;
    [SerializeField] private GameObject normalJumpParticles;
    [SerializeField] private ParticleSystem slimeTrail;
    [SerializeField] private Transform grounCheckTrans;

    /*PS4 Screen Debugs*/
    public Text text1, text2, text3;

    private CharacterController2D controller;
    private Player player;
	private float horizontalMove;
	private bool jump = false;
	private bool wallJump = true;
	
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

#if UNITY_PS4
            text1.text = "Last orientation: " + PS4Input.PadGetLastOrientation(0);
            text2.text = "Last acceleration: " + PS4Input.PadGetLastAcceleration(0);
            text3.text = "Last gyro: " + PS4Input.PadGetLastGyro(0);

            
            horizontalMove = -PS4Input.PadGetLastAcceleration(0).x * speed;
            jump = PS4Input.PadGetLastAcceleration(0).y < 0.3f;

#elif UNITY_EDITOR_WIN || UNITY_STANDALONE
            horizontalMove = Input.GetAxis("Horizontal") * speed;
#endif
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

	}

    public float GetSpeed()
    {
        return speed;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    
	void FixedUpdate()
	{
		controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
		jump = false;
	}
}
