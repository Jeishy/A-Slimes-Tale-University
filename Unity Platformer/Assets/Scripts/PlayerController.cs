using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] float speed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float gravity;
    [SerializeField] float wallDetectDist;
    [SerializeField] float wallFriction;


    CharacterController controller;
    Vector3 movement;

    bool canJump = true;
    bool hasWallJumped = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {

        Movement();

    }

    void Movement()
    {

        if (controller.isGrounded)
        {
            hasWallJumped = false;
            canJump = true;
        }

        //Calculate horizontal movement
        movement.x = Input.GetAxis("Horizontal") * speed;

        WallFriction();
        WallJumping();

        if (canJump)

        {
            Jump();
        }

        //Calculate vertical movement
        movement.y -= (gravity * Time.deltaTime) * wallFriction;


        //Move character based on calculated movement above
        controller.Move(movement);

        

    }

    void WallFriction()
    {

        //Check if player is touching wall, if its in the air and if is dropping down
        if (IsTouchingWall() && !controller.isGrounded && controller.velocity.y < 0)
        {
            Debug.Log("Applying wall friction");

            //Apply gravity multiplier
            wallFriction = 0.002f;
        } else
        {
            //Revert gravity multiplier
            wallFriction = 1;
        }
    }

    void Jump()
    {
        if (Input.GetButton("Jump"))
        {
            movement.y = jumpSpeed;
            canJump = false;
        }
    }

    void WallJumping()
    {
        if (IsTouchingWall() && !controller.isGrounded && !hasWallJumped)
        {
            canJump = true;
            hasWallJumped = true;
        }
    }


    bool IsTouchingWall()
    {

        RaycastHit hit;
        return (Physics.Raycast(transform.position, Vector3.right, out hit, wallDetectDist) || Physics.Raycast(transform.position, Vector3.left, out hit, wallDetectDist));
        
    }

}