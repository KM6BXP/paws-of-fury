using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_movement : MonoBehaviour
{
    public bool useArrows;

    CharacterController characterController;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 0.0f;

    private Vector3 moveVelocity = Vector3.zero;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Get input
        if (useArrows)
        {
            moveVelocity.x = Input.GetAxis("HorizontalArrow") * speed;
        }

        if (!useArrows)
        {
            moveVelocity.x = Input.GetAxis("HorizontalWASD") * speed;
        }

        if ((characterController.collisionFlags & CollisionFlags.Above) != 0 && moveVelocity.y > 0)
        {
            moveVelocity.y = 0;
        }

        //Jumping
        if ((characterController.collisionFlags & CollisionFlags.Below) != 0)
        {
            moveVelocity.y = 0;
            if (useArrows)
            {
                if (Input.GetAxis("VerticalArrow") > 0)
                {
                    moveVelocity.y += jumpSpeed;
                }
            }

            if (!useArrows)
            {
                if (Input.GetAxis("VerticalWASD") > 0)
                {
                    moveVelocity.y += jumpSpeed;
                }
            }
        }
        // Apply gravity when not on the ground. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveVelocity.y -= gravity * Time.deltaTime;





        // Move the controller
        Vector3 moveDirection = moveVelocity * Time.deltaTime;
        characterController.Move(moveDirection);

        //Make sure to never ever move on the z axis  or bad stuff will happen
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}