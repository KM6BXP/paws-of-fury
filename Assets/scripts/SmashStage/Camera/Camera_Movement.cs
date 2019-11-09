using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Camera_Movement : MonoBehaviour
{
    public bool dynamic_camera = true;

    public List<Transform> targets;
    public Vector3 offset;
    //how smooth it should be
    public float smoothTime = 0.5f;
    //how big should the Δy be for the offset to cancel
    public float offset_falloff = 5;

    //this controls the zoom
    public float minZoom = 40f;
    public float maxZoom = 10f;
    public float limitZoom = 50f;

    private Bounds bounds;
    private Vector3 velocity;
    private Camera cam;

    public void Start()
    {
        //get components and calculate bounds, fun fact something here is wrong and creates the zoom in effect when the level is loaded
        GetBounds();
        cam = GetComponent<Camera>();
        transform.position = bounds.center + offset;
    }

    //updates the camera in the LateUpdate because we want to make sure this executes after moving the players
    private void LateUpdate()
    {
        if (targets.Count == 0 || !dynamic_camera)
            return;
        GetBounds();
        Move();
        Zoom();
    }

    //sets the zoom
    private void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, Mathf.Max(bounds.size.x, bounds.size.y) / limitZoom);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    //handles the motion
    private void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newOffset = offset;
        if(bounds.size.y > offset_falloff)
        {
            newOffset.y -= offset.y;
        }
        Vector3 newPosition = centerPoint + newOffset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    //this returns the bounds of the characters
    void GetBounds()
    {
        bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
    }

    //this gets the centre
    Vector3 GetCenterPoint()
    {
        if(targets.Count == 1)
        {
            return targets[0].position;
        }
        return bounds.center;
    }
}
