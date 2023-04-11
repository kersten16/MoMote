using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{

    //public Vector2 lastPinch = new Vector2(0,0);
    public GameObject CameraParent;
    public modelViewer viewer;
    public GameObject cube;
    public GameObject radialMenu;

    public Vector2 lastRotPosition;
    public float lastDistance = 0;
    public float distance;
    
    public float rotateSpeed = 15f;
   // public float RotateSpeedTouch = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (!radialMenu.activeSelf)
        {
            if (Input.touchCount == 2){
                var touch0 = Input.GetTouch(0);
                var touch1 = Input.GetTouch(1);
            
                distance = Vector2.Distance(touch1.position, touch0.position);

                if (Mathf.Abs(lastDistance-distance) >= 50) lastDistance = distance; // for avoiding sharp zooms on first touch
                viewer.zoom((lastDistance - distance) * 0.001f);

                lastDistance = distance;
            } else if (Input.touchCount>0){
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    lastRotPosition = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    Vector3 offset = touch.position - lastRotPosition;
                    float RotateSpeedTouch = touch.deltaTime * 10;
                    lastRotPosition = touch.position;
                    Rotate(offset.x * RotateSpeedTouch, offset.y * RotateSpeedTouch);
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    lastRotPosition = new Vector2();
                }
            }
        }
        // } else if (Input.touchCount > 0){
        //     touch = Input.GetTouch(0);
        //     if (touch.phase == TouchPhase.Began)
        //     {
        //         oldTouchPosition = touch.position;
        //     }

        //     else if (touch.phase == TouchPhase.Moved)
        //     {
        //         NewTouchPosition = touch.position;
        //     }

        //     Vector2 rotDirection = oldTouchPosition - NewTouchPosition;
        //     Debug.Log(rotDirection);
        //     if (rotDirection.x < 0 )
        //     {
        //         RotateRight();
        //     }

        //     else if (rotDirection.x > 0 )
        //     {
        //         RotateLeft();
        //     }
        // }
    //          else if (Input.touchCount > 0)
    //  {
    //      Touch touch = Input.GetTouch(0);
    //      switch (touch.phase)
    //      {
    //          case TouchPhase.Began:
    //              _startingPosition = touch.position.x;
    //              break;
    //          case TouchPhase.Moved:
    //              if (_startingPosition > touch.position.x)
    //              {
    //                 cube.transform.Rotate(Vector3.back, -rotatespeed * Time.deltaTime);
    //              }
    //              else if (_startingPosition < touch.position.x)
    //              {
    //                 cube.transform.Rotate(Vector3.back, rotatespeed * Time.deltaTime);
    //              }
    //              break;
    //          case TouchPhase.Ended:
    //              break;
    //         }
    //     }
    }

    void Rotate(float x, float y){
        float rotX = x * rotateSpeed * Mathf.Deg2Rad;
        float rotY = y * rotateSpeed * Mathf.Deg2Rad;
        //cube.transform.Rotate(Vector3.up, -rotX);
        //cube.transform.Rotate(Vector3.right, rotY);
        CameraParent.transform.RotateAround(cube.transform.position, CameraParent.transform.right, -rotY);
        CameraParent.transform.RotateAround(cube.transform.position, CameraParent.transform.up, rotX);
    }

    // void RotateLeft(){
    //     cube.transform.rotation = Quaternion.Euler(0f, 1.5f * keepRotateSpeed, 0f) * transform.rotation;
    // }

    // void RotateRight()
    // {
    //     cube.transform.rotation = Quaternion.Euler(0f, -1.5f * keepRotateSpeed, 0f) * transform.rotation;
    // }
}
