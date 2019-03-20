using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private readonly float MoveSpeed = 1f;
    private readonly float MinZoom = 3f;
    private readonly float MaxZoom = 9f;
    private readonly float MinY = -39f;
    private readonly float MaxY = 1f;
    private float MouseStartY;
    private float MouseMoveY;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {

    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        CheckArrowMovement();
        CheckMouseWheelZoom();
        CheckDrag();
    }

    /// <summary>
    /// Moves the camera if any arrow is pressed
    /// </summary>
    private void CheckArrowMovement()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - MoveSpeed, transform.position.z);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + MoveSpeed, transform.position.z);
        }
    }

    /// <summary>
    /// Zooms in or out if the wheel of the mouse is used
    /// </summary>
    private void CheckMouseWheelZoom()
    {
        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");

        if (mouseWheel > 0)
        {
            if (Camera.main.orthographicSize > MinZoom)
            {
                Camera.main.orthographicSize--;
            }
        }
        else if (mouseWheel < 0)
        {
            if (Camera.main.orthographicSize < MaxZoom)
            {
                Camera.main.orthographicSize++;
            }
        }
    }

    /// <summary>
    /// Check if the player is dragging the map to move the camera
    /// </summary>
    private void CheckDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MouseStartY = Input.mousePosition.y;
        }
        else if (Input.GetMouseButton(0))
        {
            MouseMoveY = Input.mousePosition.y - MouseStartY;
            MouseStartY = Input.mousePosition.y;
            transform.position = new Vector3(transform.position.x, transform.position.y + MouseMoveY * transform.position.z * 0.005f, transform.position.z);
        }
    }

}


