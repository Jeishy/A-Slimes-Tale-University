using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] float speed = 0.3f;
    [SerializeField] float jumpSpeed = 8.0f;
    [SerializeField] float gravity = 20.0f;

    CharacterController controller;
    Vector3 movement;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {

        movement.x = Input.GetAxis("Horizontal") * speed;


        if (controller.isGrounded)

        {
            if (Input.GetButton("Jump"))
            {
                movement.y = jumpSpeed;
            }
        }

        movement.y -= gravity * Time.deltaTime;

        controller.Move(movement);

    }
}



