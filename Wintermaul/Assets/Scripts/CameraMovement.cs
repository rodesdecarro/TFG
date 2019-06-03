using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed = 0f;

    [SerializeField]
    private float dragSpeed = 0f;

    private float xMax;
    private float yMin;

    private float MouseStartY;
    private float MouseMoveY;
    private float MouseStartX;
    private float MouseMoveX;

    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = transform.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            camera.orthographicSize = Mathf.Min(camera.orthographicSize + 1, 12);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            camera.orthographicSize = Mathf.Max(5, camera.orthographicSize - 1);
        }

        float newX = 0;
        float newY = 0;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newY += cameraSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newX -= cameraSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newY -= cameraSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newX += cameraSpeed * Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(1))
        {
            MouseStartY = Input.mousePosition.y;
            MouseStartX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButton(1))
        {
            MouseMoveY = Input.mousePosition.y - MouseStartY;
            MouseMoveX = Input.mousePosition.x - MouseStartX;

            MouseStartY = Input.mousePosition.y;
            MouseStartX = Input.mousePosition.x;


            newY -= MouseMoveY * dragSpeed * Time.deltaTime;
            newX -= MouseMoveX * dragSpeed * Time.deltaTime;
        }

        if (newX != 0 || newY != 0)
        {
            transform.Translate(new Vector3(newX, newY));
        }

        float difMinX = (Camera.main.ViewportToWorldPoint(new Vector3(0, 1)).x - minTile.x) + 1;
        float difMinY = (Camera.main.ViewportToWorldPoint(new Vector3(0, 1)).y - minTile.y) - 1;
        float difMaxX = (Camera.main.ViewportToWorldPoint(new Vector3(1, 0)).x - maxTile.x) - 1;
        float difMaxY = (Camera.main.ViewportToWorldPoint(new Vector3(1, 0)).y - maxTile.y) + 1;

        if (difMinX < 0)
        {
            transform.Translate(Vector3.left * difMinX);
        }
        else if (difMaxX > 0 && Mathf.Abs(difMinX) > 0.001)
        {
            newX = Mathf.Min(newX, -difMinX);
            transform.Translate(Vector3.left * difMaxX);
        }

        if (difMinY > 0)
        {
            transform.Translate(Vector3.down * difMinY);
        }
        else if (difMaxY < 0 && Mathf.Abs(difMinY) > 0.001)
        {
            transform.Translate(Vector3.down * difMaxY);
        }
    }

    private Vector3 maxTile;
    private Vector3 minTile;

    public void SetLimits(Vector3 maxTile, Vector3 minTile)
    {
        this.minTile = minTile;
        this.maxTile = maxTile;
    }
}
