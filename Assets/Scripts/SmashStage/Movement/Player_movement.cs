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
    public float gravity = 9.81f;

    public float weight = 20f;

    private bool m_isJumpInUse = false;

    public Vector3 moveVelocity = Vector3.zero;
    private int jump = 0;
    void Start()
    {
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

        //Jumping
        if (characterController.isGrounded)
        {
            //dont want to move down when touching ground
            moveVelocity.y = 0;
            jump = 0;
        }
        // Always apply gravity cuz unity weird. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveVelocity.y -= gravity * Time.deltaTime * weight;
        if (jump < 1 && !characterController.isGrounded)
            jump = 1;

        if (!m_isJumpInUse && jump < 2)
        {
            if (useArrows)
            {
                if (Input.GetAxis("VerticalArrow") > 0)
                {
                    moveVelocity.y = jumpSpeed;
                    jump++;
                }
            }

            if (!useArrows)
            {
                if (Input.GetAxis("VerticalWASD") > 0)
                {
                    moveVelocity.y = jumpSpeed;
                    jump++;
                }
            }
            m_isJumpInUse = true;
        }
        if (useArrows && Input.GetAxis("VerticalArrow") == 0)
            m_isJumpInUse = false;

        if (!useArrows && Input.GetAxis("VerticalWASD") == 0)
            m_isJumpInUse = false;


        // Move the controller
        Vector3 moveDirection = moveVelocity * Time.deltaTime;
        characterController.Move(moveDirection);

        //Make sure to never ever move on the z axis  or bad stuff will happen
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}