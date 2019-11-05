﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Camera_Movement : MonoBehaviour
{
    public bool dynamic_camera = true;

    public List<Transform> targets;
    public Vector3 offset;
    public float smoothTime = 0.5f;
    public float offset_falloff = 5;

    public float minZoom = 40f;
    public float maxZoom = 10f;
    public float limitZoom = 50f;

    private Bounds bounds;
    private Vector3 velocity;
    private Camera cam;

    public void Start()
    {
        GetBounds();
        cam = GetComponent<Camera>();
        transform.position = bounds.center + offset;
    }

    private void LateUpdate()
    {
        if (targets.Count == 0 || !dynamic_camera)
            return;
        GetBounds();
        Move();
        Zoom();
    }

    private void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, Mathf.Max(bounds.size.x, bounds.size.y) / limitZoom);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

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

    void GetBounds()
    {
        bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
    }


    Vector3 GetCenterPoint()
    {
        if(targets.Count == 1)
        {
            return targets[0].position;
        }
        return bounds.center;
    }
}
