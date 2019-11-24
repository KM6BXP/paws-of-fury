using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platform_movement : MonoBehaviour
{
    Vector3 pos;
    public bool X;
    public float distanceX = 1;
    public float speedX = 1;
    public float offsetX = 0;

    public bool Y;
    public float distanceY = 1;
    public float speedY = 1;
    public float offsetY = 0;

    public bool Z;
    public float distanceZ = 1;
    public float speedZ = 1;
    public float offsetZ = 0;

    private Rigidbody rbody;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        rbody = transform.GetComponent<Rigidbody>();
    }

    // This is not that hard, just a sine wave with an amplitude of distance and a wavelength of speed
    //sin(x*speed)*distance
    void Update()
    {
        Vector3 moveVector = pos;
        if (Y)
            moveVector.y = pos.y + Mathf.Sin(Time.time * speedY + offsetY) * distanceY;
        if (X)
            moveVector.x = pos.x + Mathf.Sin(Time.time * speedX + offsetX) * distanceX;
        if (Z)
            moveVector.z = pos.z + Mathf.Sin(Time.time * speedZ + offsetZ) * distanceZ;
        rbody.MovePosition(moveVector);
    }

    void OnTriggerStay(Collider hit)
    {
        if (hit.tag == "Player")
        {
            if (hit.GetComponent<Player_movement>().isGrounded())
            {
                hit.transform.parent = transform;
            }
        }
    }
    void OnTriggerExit(Collider hit)
    {
        if (hit.tag == "Player")
        {
            if (!hit.GetComponent<Player_movement>().isGrounded())
            {
                hit.transform.parent = null;
            }
        }

    }
}
