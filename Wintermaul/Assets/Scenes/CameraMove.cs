using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float speed = 10f;

    private Vector3 MouseStart, MouseMove;
    private Vector3 derp;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - speed, transform.position.z);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + speed, transform.position.z);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
        }

        Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * 10;

        /*
        if (Input.GetMouseButtonDown(0))
        {
            //create a variable to hold the mouse position just because it looks clearer and easier to read
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //now we create a second vector3 as we don't want the sprite z axis to match the mouse position only on X and Y we want to stay on the same z axis
            Vector3 position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
            //set its position equal to the vector3 we just created
            transform.position = position;
        }
        */

        if (Input.GetMouseButtonDown(0))
        {
            MouseStart = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z);
        }
        else if (Input.GetMouseButton(0))
        {
            MouseMove = new Vector3(Input.mousePosition.x - MouseStart.x, Input.mousePosition.y - MouseStart.y, transform.position.z);
            MouseStart = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z);
            transform.position = new Vector3(transform.position.x + MouseMove.x * Time.deltaTime * transform.position.z, transform.position.y + MouseMove.y * Time.deltaTime * transform.position.z, transform.position.z);
        }
    }

}


