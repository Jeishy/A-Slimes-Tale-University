using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private bool DEBUG = false;
    
    [SerializeField] private float speed = 0.3f;
    [SerializeField] private float verticalJumpForce = 0.3f;
    [SerializeField] private float horizontalJumpForce = 0.1f;
    [SerializeField] private float gravity = -1;
    [SerializeField] private float wallDetectDist = 0.6f;
    [SerializeField] private float wallSlideSpeedMax = 0.01f;
    
    //[SerializeField] float wallJumpForceDuration = 1f;

    private CharacterController controller;
    private Vector3 velocity;

    private float vSpeed = 0;
    private float hSpeed = 0;
    private float wallJumpForce;
    
    private float wallJumpForceTime;
    [SerializeField] private float wallJumpDampenForce = 0.01f;

    private bool canJump = true;
    private bool hasWallJumped = false;

    private int layer_mask;

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
            wallJumpForce = 0;
        }

        if (Input.GetKey(KeyCode.A) && wallJumpForce > 0 || Input.GetKey(KeyCode.D) && wallJumpForce < 0)
        {
            wallJumpForce = 0;
        }
        
        //Calculate horizontal movement
        hSpeed = Input.GetAxis("Horizontal") * speed;


        //Check if player can wall jump 
        WallJumping();


        //Jumping
        if (canJump && Input.GetButtonDown("Jump"))
        {
            //If the player is on the wall, the character is also launched horizontally away from the wall
            if (!controller.isGrounded)
            {
                if (DEBUG) Debug.Log("Jumping off wall - wall normal: " + GetWallNormal());
                
                wallJumpForce = horizontalJumpForce * GetWallNormal().x;
                //RELATED TO FUNCTION BELOW, MIGHT BE USEFUL LATER
                //StartCoroutine(DampedJumpForce(wallJumpForce));
            }


            //Apply force vertically
            vSpeed = verticalJumpForce;
            canJump = false;
            if (DEBUG) Debug.Log("Jump");
        }





        //Calculate wall friction
        WallFriction();
        //Calculate gravity
        vSpeed += gravity * Time.deltaTime;

        //Apply vertical movement
        velocity.y = vSpeed;
        velocity.x = hSpeed + wallJumpForce;

        if (DEBUG) Debug.Log("HSpeed: " + hSpeed + " | Wall Jump Force: " + wallJumpForce);
        
        //Move character based on calculated movement above
        controller.Move(velocity);


    }

    /*
     MIGHT BE USEFUL LATER
     
     IEnumerator DampedJumpForce(float startingForce)
    {
        float force = startingForce;
        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / wallJumpForceDuration;
            wallJumpForce = Mathf.Lerp(startingForce, 0, t);
            yield return null;
        }

    }*/
    
    
    void WallFriction()
    {

        //Check if player is touching wall, if its in the air and if is dropping down
        if (IsTouchingWall() && !controller.isGrounded && vSpeed < 0)
        {

            //Limit wall sliding speed
            if (vSpeed < -wallSlideSpeedMax)
            {
                vSpeed = -wallSlideSpeedMax;
                wallJumpForce = 0;
            }
        }
    }


    //Check if player can wall jump
    void WallJumping()
    {        
        if (IsTouchingWall() && !controller.isGrounded && !hasWallJumped)
        {
            //Set variables to allow to jump once more and prevent from double jumping mid-air
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
            if (DEBUG) Debug.Log("Collected!");
            Destroy(other.gameObject);
        }
    }

    

}
