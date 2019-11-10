using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platform_movement : MonoBehaviour
{
    Vector3 pos;
    public float distance = 1;
    public float speed = 1;

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
        rbody.MovePosition(new Vector3(pos.x , pos.y + Mathf.Sin(Time.time * speed) * distance, pos.z));
    }

    void OnTriggerEnter(Collider hit)
    {
        hit.transform.parent = transform;
    }

    void OnTriggerExit(Collider hit)
    {
        hit.transform.parent = null;
    }
}
