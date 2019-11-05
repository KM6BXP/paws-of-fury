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

    private Vector3 moveDirection = Vector3.zero;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //get input
        if (useArrows)
        {
            moveDirection = new Vector3(Input.GetAxis("HorizontalArrow"), 0.0f, 0.0f);
        }

        if (!useArrows)
        {
            moveDirection = new Vector3(Input.GetAxis("HorizontalWASD"), 0.0f, 0.0f);
        }

        moveDirection *= speed;

        if (characterController.isGrounded)
        {
            if (useArrows)
            {
                if (Input.GetAxis("VerticalArrow") > 0)
                {
                    moveDirection.y = jumpSpeed;
                }
            }

            if (!useArrows)
            {
                if (Input.GetAxis("VerticalWASD") > 0)
                {
                    moveDirection.y = jumpSpeed;
                }
            }
        }
        

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}