using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] float maxHSpeed;
    [SerializeField] float verticalJumpForce;
    [SerializeField] float horizontalJumpForce;
    [SerializeField] float gravity;
    [SerializeField] float wallDetectDist;
    [SerializeField] float wallFriction;
    [SerializeField] float wallSlideSpeedMax;


    CharacterController controller;
    Vector3 velocity;

    float vSpeed = 0;
    float hSpeed = 0;

    float wallJumpForce;

    bool wallSliding = false;
    bool canJump = true;
    bool hasWallJumped = false;

    Rigidbody rb;
    int layer_mask;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        layer_mask = LayerMask.GetMask("Wall");
    }

    void Update()
    {

        //Reset variables when player is on the ground
        if (controller.isGrounded)
        {
            
            Debug.Log("Resetting variables");
            vSpeed = 0;
            hSpeed = 0;
            hasWallJumped = false;
            canJump = true;
            wallJumpForce = horizontalJumpForce;
        }

        //Calculate horizontal movement
        velocity.x = Input.GetAxis("Horizontal") * speed;

        if (hSpeed != 0)
        {
            wallJumpForce -= Time.deltaTime;
        }

        //Check if player can wall jump 
        WallJumping();


        //Jumping
        if (canJump && Input.GetButtonDown("Jump"))
        {

            if (!controller.isGrounded)
            {
                Debug.Log("Jumping off wall - wall normal: " + GetWallNormal());
                hSpeed = wallJumpForce * GetWallNormal().x;
            }

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


        wallSliding = false;

        //Check if player is touching wall, if its in the air and if is dropping down
        if (IsTouchingWall() && !controller.isGrounded && vSpeed < 0)
        {
            wallSliding = true;

            if (vSpeed < -wallSlideSpeedMax)
            {
                vSpeed = -wallSlideSpeedMax;
            }
        }
    }


    void WallJumping()
    {
        if (IsTouchingWall() && !controller.isGrounded && !hasWallJumped)
        {
            //Debug.Log("Player can jump again - touching: " + IsTouchingWall() + " , grounded: " + controller.isGrounded + " , wall jumped: " + hasWallJumped);
            canJump = true;
            hasWallJumped = true;
        }
    }


    bool IsTouchingWall()
    {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.right, out hit, wallDetectDist, layer_mask) || Physics.Raycast(transform.position, Vector3.left, out hit, wallDetectDist, layer_mask))
        {
            //Debug.Log("Touched wall: " + hit.collider.gameObject.name);
            return true;
        }

        return false;

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
            Debug.Log("Collected 2!");
            Destroy(other.gameObject);
        }
    }

}
