using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_movement : MonoBehaviour
{
    public bool useArrows;

    CharacterController characterController;

    //Set per character and per level information
    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 9.81f;

    public float weight = 20f;

    //Jumping
    private float coyoteTime = 0;
    private const float const_maxCoyoteTime = 0.1f;
    private int jump = 0;
    private bool m_isJumpInUse = false;
    public Vector3 moveVelocity = Vector3.zero;

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

        //Make sure object doesn't stick to a ceiling
        if ((characterController.collisionFlags & CollisionFlags.Above) != 0 && moveVelocity.y > 0)
        {
            moveVelocity.y = 0;
        }

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

        //Jumping
        if (isGrounded() && transform.parent == null)
        {
            //Dont want to move down when touching ground
            moveVelocity.y = -gravity * Time.deltaTime * weight; ;
        }
        if (isGrounded())
        {
            jump = 0;
        }
        if(!isGrounded())
        {
            // Apply gravity when not on the ground. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            moveVelocity.y -= gravity * Time.deltaTime * weight;
            if (jump < 1)
                jump = 1;
        }


        // Move the controller
        Vector3 moveDirection = moveVelocity * Time.deltaTime;
        characterController.Move(moveDirection);

        //Make sure to never ever move on the z axis  or bad stuff will happen
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        //This is to smooth isGrounded so it isn't freaking out
        if (characterController.isGrounded)
        {
            coyoteTime = 0;
        }
        else if(coyoteTime <= const_maxCoyoteTime)
        {
            coyoteTime += Time.deltaTime;
        }
    }

    //Return a smoothed isGrounded and looks at velocity.y
    public bool isGrounded()
    {
        return coyoteTime <= const_maxCoyoteTime && moveVelocity.y <= 0;
    }
}