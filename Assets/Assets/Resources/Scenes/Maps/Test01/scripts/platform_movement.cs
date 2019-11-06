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

    // Update is called once per frame
    void Update()
    {
        rbody.MovePosition(new Vector3(pos.x , pos.y + Mathf.Sin(Time.time * speed) * distance, pos.z));
    }
}
