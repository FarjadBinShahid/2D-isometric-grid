using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float CameraSpeed = 1f;

    Camera cam;
    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        


        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * CameraSpeed);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * CameraSpeed);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * CameraSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * CameraSpeed);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            cam.orthographicSize += CameraSpeed;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            cam.orthographicSize -= CameraSpeed;
        }


    }
}
