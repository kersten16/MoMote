using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class modelViewer : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera cam;
    public GameObject loadedModel;
    public GameObject radialMenu;
    Bounds bounds;
    bool loaded = false;
    void Start()
    {
          loaded = false;
          radialMenu.SetActive(false);
    }

    public void setCam(){
        Renderer[] renderers = loadedModel.GetComponentsInChildren<Renderer>();
        bounds = new Bounds(loadedModel.transform.position, Vector3.one);
        foreach (Renderer r in renderers)
        {              
            bounds.Encapsulate(r.bounds);
        }
        Debug.Log(bounds);
        float cameraDistance = 0.50f; // Constant factor
        Vector3 objectSizes = bounds.max - bounds.min;
        float objectSize = Mathf.Max(objectSizes.x, objectSizes.y, objectSizes.z);
        Debug.Log(objectSize);
        float cameraView = 2.0f * Mathf.Tan(0.5f * Mathf.Deg2Rad * cam.fieldOfView); // Visible height 1 meter in front
        float distance = cameraDistance * objectSize / cameraView; // Combined wanted distance from the object
        distance += 0.5f * objectSize; // Estimated offset from the center to the outside of the object
        cam.transform.position = bounds.center - distance * cam.transform.forward; 
        loaded = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (loaded){
            // todo get normal between previous input and current input
            // todo rotation angle = delta between previous input and current input
            //rotate(new Vector3(1,1,1), 20*Time.deltaTime);
            if(Input.GetKey(KeyCode.Z)){
                zoom(0.1f);
            }
            if(Input.GetKey(KeyCode.S)){
                zoom(-0.1f);
            }
            if(Input.GetKeyDown(KeyCode.M)){
                radialMenu.SetActive(!radialMenu.activeSelf);
            }
        }
        
    }

    void rotate(Vector3 axis, float angle){
            transform.RotateAround(bounds.center, axis, angle);
    }

    void zoom(float z){
        var distance = Vector3.Distance(transform.position, bounds.center);
        if ( (z > 0 && distance > 0.4f) || ( z < 0 && distance < 50))
            transform.Translate(Vector3.forward*z);
    }
}
