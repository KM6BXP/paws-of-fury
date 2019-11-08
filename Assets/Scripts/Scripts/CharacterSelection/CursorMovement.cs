using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorMovement : MonoBehaviour {

    public float speed;
    private Vector3 worldSize;

    void Start()
    {
        worldSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }

    void Update () {

        float x = Input.GetAxis("HorizontalWASD");
        float y = Input.GetAxis("VerticalWASD");

        transform.position += new Vector3(x, y, 0) * Time.deltaTime * speed;

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -worldSize.x, worldSize.x),
            Mathf.Clamp(transform.position.y, -worldSize.y, worldSize.y),
            transform.position.z);
    }
}
