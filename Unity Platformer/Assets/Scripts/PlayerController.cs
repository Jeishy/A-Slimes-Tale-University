using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float speed;
    public float verticalJumpForce;
    [SerializeField] float horizontalJumpForce;
    public float gravity;
    [SerializeField] float wallDetectDist;
    [SerializeField] float wallFriction;
    [SerializeField] float wallSlideSpeedMax;

    CharacterController controller;
    Vector3 velocity;

    float vSpeed = 0;
    float hSpeed = 0;
    float wallJumpForce;

    bool canJump = true;
    bool hasWallJumped = false;

    int layer_mask;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        layer_mask = LayerMask.GetMask("Wall");
    }

    void Update()
    {


        


        //Reset variables when player is on the ground
        if (controller.isGrounded)
        {
            vSpeed = 0;
            hSpeed = 0;
            hasWallJumped = false;
            canJump = true;
            wallJumpForce = horizontalJumpForce;
        }

        //Calculate horizontal movement
        velocity.x = Input.GetAxis("Horizontal") * speed;

        //Dampen horizontal speed - air resistance
        if (hSpeed != 0)
        {
            wallJumpForce -= Time.deltaTime;
        }

        //Check if player can wall jump 
        WallJumping();


        //Jumping
        if (canJump && Input.GetButtonDown("Jump"))
        {
            //If the player is on the wall, the character is also launched horiontally away from the wall
            if (!controller.isGrounded)
            {
                Debug.Log("Jumping off wall - wall normal: " + GetWallNormal());
                hSpeed = wallJumpForce * GetWallNormal().x;
            }


            //Apply force vertically
            vSpeed = verticalJumpForce;
            canJump = false;
            Debug.Log("Jump");
        }





        //Calculate wall friction
        WallFriction();
        //Calculate gravity
        vSpeed += gravity * Time.deltaTime;

        //Apply vertical movement
        velocity.y = vSpeed;
        velocity.x += hSpeed;

        //Move character based on calculated movement above
        controller.Move(velocity);


    }

    void WallFriction()
    {

        //Check if player is touching wall, if its in the air and if is dropping down
        if (IsTouchingWall() && !controller.isGrounded && vSpeed < 0)
        {

            //Limit wall sliding speed
            if (vSpeed < -wallSlideSpeedMax)
            {
                vSpeed = -wallSlideSpeedMax;
            }
        }
    }


    //Check if player can wall jump
    void WallJumping()
    {
        if (IsTouchingWall() && !controller.isGrounded && !hasWallJumped)
        {
            //Set variables to allow to jump once more and prevent from jumping again
            canJump = true;
            hasWallJumped = true;
        }
    }


    bool IsTouchingWall()
    {

        //Check if player touches the wall by casting a ray in the left and right direction
        return (Physics.Raycast(transform.position, Vector3.right, wallDetectDist, layer_mask) || Physics.Raycast(transform.position, Vector3.left, wallDetectDist, layer_mask));

    }


    Vector3 GetWallNormal()
    {

        Vector3 wallNormal = Vector2.zero;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.right, out hit, wallDetectDist, layer_mask) || Physics.Raycast(transform.position, Vector3.left, out hit, wallDetectDist, layer_mask))
        {
            wallNormal = hit.normal;
        }

        return wallNormal;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            Debug.Log("Collected!");
            Destroy(other.gameObject);
        }
    }

}
