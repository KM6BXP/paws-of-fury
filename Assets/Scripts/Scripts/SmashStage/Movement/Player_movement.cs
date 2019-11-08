using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_movement : MonoBehaviour
{
    public bool useArrows;

    CharacterController characterController;

    //set per character and per level information
    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 0.0f;

    public float JumpCooldown = 1f;
    private float timer;

    private Vector3 moveVelocity = Vector3.zero;
    private int jump = 0;
    void Start()
    {
        timer = JumpCooldown;
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        //Get horizontal input
        if (useArrows)
        {
            moveVelocity.x = Input.GetAxis("HorizontalArrow") * speed;
        }

        if (!useArrows)
        {
            moveVelocity.x = Input.GetAxis("HorizontalWASD") * speed;
        }

        //make sure object doesn't stick to a ceiling
        if ((characterController.collisionFlags & CollisionFlags.Above) != 0 && moveVelocity.y > 0)
        {
            moveVelocity.y = 0;
        }

        //double jump here, otherwise the character will jump immediately again
        //also countdown to prevent jumping twice in 2 frames
        if(timer <= 0) {
            if (jump == 1)
            {
                jump++;
                if (useArrows)
                {
                    if (Input.GetAxis("VerticalArrow") > 0)
                    {
                        moveVelocity.y = jumpSpeed;
                    }
                }

                if (!useArrows)
                {
                    if (Input.GetAxis("VerticalWASD") > 0)
                    {
                        moveVelocity.y = jumpSpeed;
                    }
                }
            }
        }
        else
        {
            timer -= Time.deltaTime;
        }

        //Jumping
        if ((characterController.collisionFlags & CollisionFlags.Below) != 0)
        {
            //dont want to move down when touching ground
            moveVelocity.y = 0;
            jump = 0;

            
            if (useArrows)
            {
                if (Input.GetAxis("VerticalArrow") > 0)
                {
                    moveVelocity.y = jumpSpeed;
                    jump++;
                    timer = JumpCooldown;
                }
            }

            if (!useArrows)
            {
                if (Input.GetAxis("VerticalWASD") > 0)
                {
                    moveVelocity.y = jumpSpeed;
                    jump++;
                    timer = JumpCooldown;
                }
            }
        }
        else
        {
            // Apply gravity when not on the ground. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            moveVelocity.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        Vector3 moveDirection = moveVelocity * Time.deltaTime;
        characterController.Move(moveDirection);

        //Make sure to never ever move on the z axis  or bad stuff will happen
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}