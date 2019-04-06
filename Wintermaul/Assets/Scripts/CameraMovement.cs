using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed;

    private float xMax;
    private float yMin;

    private float MouseStartY;
    private float MouseMoveY;
    private float MouseStartX;
    private float MouseMoveX;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.up * cameraSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * cameraSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.down * cameraSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * cameraSpeed * Time.deltaTime);
        }

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


            transform.Translate(Vector3.down * MouseMoveY * Time.deltaTime);
            transform.Translate(Vector3.left * MouseMoveX * Time.deltaTime);
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0, xMax), Mathf.Clamp(transform.position.y, yMin, 0), transform.position.z);
    }

    public void SetLimits(Vector3 maxTile)
    {
        Vector3 worldPoint = Camera.main.ViewportToWorldPoint(new Vector3(1, 0));

        xMax = Mathf.Max(0, maxTile.x - worldPoint.x);
        yMin = Mathf.Min(0, maxTile.y - worldPoint.y);
    }
}
