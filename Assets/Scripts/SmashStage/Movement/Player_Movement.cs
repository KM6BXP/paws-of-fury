using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Momentum))]
public class Player_Movement : MonoBehaviour
{
    public bool useArrows;

    CharacterController characterController;
    Momentum Momentum;

    //Set per character and per level information
    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 9.81f;
    public float weight = 20f;

    //Jumping
    public bool Grounded = true;

    private int jump = 0;
    private bool m_isJumpInUse = false;
    public Vector3 moveVelocity = Vector3.zero;

    //Momentum of ground
    private Vector3 floorVelocity;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Momentum = GetComponent<Momentum>();
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

        //Jumping
        if (Grounded)
        {
            //Dont want to move down when touching ground
            moveVelocity.y = -gravity * Time.deltaTime * weight;
            jump = 0;
        }
        if (!Grounded)
        {
            // Apply gravity when not on the ground. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            moveVelocity.y -= gravity * Time.deltaTime * weight;
            if (jump < 1)
                jump = 1;
        }

        if (!m_isJumpInUse && jump < 2)
        {
            if (useArrows)
            {
                if (Input.GetAxis("VerticalArrow") > 0)
                {
                    m_isJumpInUse = true;
                    moveVelocity.y = jumpSpeed;
                    jump++;
                    Grounded = false;
                }
            }

            if (!useArrows)
            {
                if (Input.GetAxis("VerticalWASD") > 0)
                {
                    m_isJumpInUse = true;
                    moveVelocity.y = jumpSpeed;
                    jump++;
                    Grounded = false;
                }
            }
        }

        if (useArrows && Input.GetAxis("VerticalArrow") == 0)
            m_isJumpInUse = false;

        if (!useArrows && Input.GetAxis("VerticalWASD") == 0)
            m_isJumpInUse = false;

        Momentum.Velocity = moveVelocity;
        // Move the controller
        Vector3 moveDirection = (moveVelocity + floorVelocity) * Time.deltaTime;
        characterController.Move(moveDirection);

        //Make sure to never ever move on the z axis  or bad stuff will happen
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    public void HandleTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Level")
            Grounded = true;
    }
    public void HandleTriggerStay(Collider collision)
    {

        if (collision.transform.GetComponent<Momentum>() && collision.transform.tag == "Level")
        {
            floorVelocity = collision.transform.GetComponent<Momentum>().Velocity;
        }
    }
    public void HandleTriggerExit(Collider collision)
    {
        if(collision.transform.tag == "Level")
            Grounded = false;
        floorVelocity = Vector3.zero;
    }
}