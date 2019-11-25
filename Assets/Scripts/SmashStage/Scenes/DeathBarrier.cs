using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    public Transform[] spawnpoints;
    void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player_Movement>().moveVelocity = Vector3.zero;
            collision.transform.position = spawnpoints[Random.Range(0, spawnpoints.Length)].position;
        }
    }
}
