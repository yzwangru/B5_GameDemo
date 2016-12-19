using UnityEngine;
using System.Collections;

public class newCam : MonoBehaviour
{
    public float moveSpeed = 50.0f; //Speed move with ASDW
    public float slowmove = 10.0f;
    public float rotateSpeed = 70.0f; //Rotate speed when press QE
    public float zoomSpeed = 80.0f; //Zoom speed when you scroll in or out
    public float zoomRotate = 40.0f; //When the camera rotate a little while zooming
    public float zoomMin = 0.0f; //Minimun value to zoom in
    public float zoomMax = 20.0f; //Maximun value to zoom out

    void Start()
    {

    }

    void Update()
    {
        #region Move camera ASDW
        //Move the camera up, down, left and right
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * Time.deltaTime * slowmove);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * Time.deltaTime * slowmove);
        }
        #endregion

        #region Zoom camera with Mouse ScrollWheel
        //Use the zoom with the scroll mouse button
        //Convert value to Int to avoid zoom problems(dont belive me?, remove it and try)
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && transform.position.y > zoomMin)
        {
            if (transform.position.y > zoomRotate)
            {
                transform.position += Vector3.down * (int)(Time.deltaTime * zoomSpeed);
            }
            else
            {
                transform.position += Vector3.down * (int)(Time.deltaTime * zoomSpeed);
                transform.Rotate(Vector3.left * (int)(Time.deltaTime * zoomSpeed));

            }
            //GetComponent<Camera>().fieldOfView--;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && transform.position.y < zoomMax)
        {
            if (transform.position.y < zoomRotate)
            {
                transform.position += Vector3.up * (int)(Time.deltaTime * zoomSpeed);
                transform.Rotate(Vector3.right * (int)(Time.deltaTime * zoomSpeed));
            }
            else
            {
                transform.position += Vector3.up * (int)(Time.deltaTime * zoomSpeed);
            }
        }
        #endregion

        #region Rotate camera with QE
        //Rotate the camera
        if (Input.GetKey(KeyCode.E))
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0, 0.5f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                transform.RotateAround(hit.point, Vector3.up, rotateSpeed * Time.deltaTime);
            }
        }

        if (Input.GetKey(KeyCode.Q))
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0, 0.5f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                transform.RotateAround(hit.point, -Vector3.up, rotateSpeed * Time.deltaTime);
            }
        }
        #endregion

    }
}