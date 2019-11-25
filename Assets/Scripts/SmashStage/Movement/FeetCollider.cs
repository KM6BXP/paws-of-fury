using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetCollider : MonoBehaviour
{
    private Player_Movement Player_Movement;

    void Start()
    {
        Player_Movement = transform.parent.GetComponent<Player_Movement>();
    }

    void OnTriggerEnter(Collider collision)
    {
        Player_Movement.HandleTriggerEnter(collision);
    }
    void OnTriggerStay(Collider collision)
    {
        Player_Movement.HandleTriggerStay(collision);
    }
    void OnTriggerExit(Collider collision)
    {
        Player_Movement.HandleTriggerExit(collision);
    }
}