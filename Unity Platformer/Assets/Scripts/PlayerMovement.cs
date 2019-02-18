using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerMovement : MonoBehaviour
{


	[SerializeField] private float speed = 40f;
	
	private CharacterController2D controller;
	private float horizontalMove;
	private bool jump = false;
	private bool wallJump = true;
	private bool crouch = false;
	
	// Use this for initialization
	void Start ()
	{
		controller = GetComponent<CharacterController2D>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		
		horizontalMove = Input.GetAxisRaw("Horizontal") * speed;

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
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

	void FixedUpdate()
	{
		controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
		jump = false;
	}

	
}
