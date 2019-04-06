using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private readonly float MoveSpeed = 0.5f;
    private readonly float DragSpeed = 0.05f;
    private readonly float MinZoom = 3f;
    private readonly float MaxZoom = 9f;
    private readonly float MinY = -39f;
    private readonly float MaxY = 1f;
    private readonly float MinX = 0f;
    private readonly float MaxX = 3f;
    private float MouseStartY;
    private float MouseMoveY;
    private float MouseStartX;
    private float MouseMoveX;

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
        CheckMovement();
        CheckMouseWheelZoom();
    }

    private void CheckMovement()
    {
        float newY = transform.position.y;
        float newX = transform.position.x;

        // Check arrows pressed
        if (Input.GetKey(KeyCode.DownArrow))
        {
            newY -= MoveSpeed;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            newY += MoveSpeed;
        }

        // Check arrows pressed
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            newX -= MoveSpeed;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            newX += MoveSpeed;
        }

        // Check drag
        if (Input.GetMouseButtonDown(0))
        {
            MouseStartY = Input.mousePosition.y;
            MouseStartX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButton(0))
        {
            MouseMoveY = Input.mousePosition.y - MouseStartY;
            MouseMoveX = Input.mousePosition.x - MouseStartX;

            MouseStartY = Input.mousePosition.y;
            MouseStartX = Input.mousePosition.x;

            newY -= MouseMoveY * DragSpeed;
            newX -= MouseMoveX * DragSpeed;
        }

        newY = Mathf.Clamp(newY, MinY, MaxY);
        newX = Mathf.Clamp(newX, MinX, MaxX);

        // Update position
        if (newX != transform.position.x || newY != transform.position.y)
        {
            transform.position = new Vector3(newX, newY, transform.position.z);
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
}


