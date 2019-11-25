using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Momentum))]
public class Platform_Movement : MonoBehaviour
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
    private Momentum momentum;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        rbody = transform.GetComponent<Rigidbody>();
        momentum = transform.GetComponent<Momentum>();
    }

    // This is not that hard, just a sine wave with an amplitude of distance and a wavelength of speed
    //sin(x*speed)*distance
    void Update()
    {
        Vector3 oldPosition = transform.position;
        Vector3 offsetVector = pos;
        if (Y)
            offsetVector.y = pos.y + Mathf.Sin(Time.time * speedY + offsetY) * distanceY;
        if (X)
            offsetVector.x = pos.x + Mathf.Sin(Time.time * speedX + offsetX) * distanceX;
        if (Z)
            offsetVector.z = pos.z + Mathf.Sin(Time.time * speedZ + offsetZ) * distanceZ;
        rbody.MovePosition(offsetVector);
        if (Y)
            momentum.Velocity.y = Mathf.Cos(Time.time * speedY + offsetY) * distanceY;
        if (X)
            momentum.Velocity.x = Mathf.Cos(Time.time * speedX + offsetX) * distanceX;
        if (Z)
            momentum.Velocity.z = Mathf.Cos(Time.time * speedZ + offsetZ) * distanceZ;
    }
}
