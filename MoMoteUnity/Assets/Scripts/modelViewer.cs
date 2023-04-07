using System;
using System.IO;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleFileBrowser;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class modelViewer : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera cam;
    public GameObject loadedModel;
    public GameObject radialMenu;
    public RMF_RadialMenu radialMenuScript;
    public MenuLoader menuLoader;
    public EventSystem ESystem;
    public AsyncOperation unloader;
    Bounds bounds;
    public bool loaded = false;
    public float zoomFactor = 10f;
    public int isZoom = 0;
    Texture2D virtualPhoto;
    private string modelName;
    private int sqr = 256;
    string FILE_PATH;
    StreamWriter writer;


    void Start()
    {
        LogManager.writeToLog("In Model Viewer");
        sqr = Screen.width;
        loaded = false;
        radialMenu.SetActive(false);
        virtualPhoto = new Texture2D(sqr, sqr, TextureFormat.RGB24, false);
    }

    public void setCam(){
        Renderer[] renderers = loadedModel.GetComponentsInChildren<Renderer>();
        bounds = new Bounds(loadedModel.transform.position, Vector3.one);
        foreach (Renderer r in renderers)
        {              
            bounds.Encapsulate(r.bounds);
        }
       // Debug.Log(bounds);
        float cameraDistance = 0.50f; // Constant factor
        Vector3 objectSizes = bounds.max - bounds.min;
        float objectSize = Mathf.Max(objectSizes.x, objectSizes.y, objectSizes.z);
       // Debug.Log(objectSize);
        float cameraView = 2.0f * Mathf.Tan(0.5f * Mathf.Deg2Rad * cam.fieldOfView); // Visible height 1 meter in front
        float distance = cameraDistance * objectSize / cameraView; // Combined wanted distance from the object
        distance += 0.5f * objectSize; // Estimated offset from the center to the outside of the object
        cam.transform.position = bounds.center - distance * cam.transform.forward;
        modelName = loadedModel.name;
        loaded = true;
        StartCoroutine(TakeSnapshot(false));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space)){
            loaded = true;
        }
        transform.position = Vector3.zero;
        if (loaded){
            switch(isZoom) 
                {
                case -1:
                    zoom(zoomFactor);
                    break;
                case 1:
                    zoom(-zoomFactor);
                    break;
                }

            // todo get normal between previous input and current input
            // todo rotation angle = delta between previous input and current input
            //rotate(new Vector3(1,1,1), 20*Time.deltaTime);
            if(Input.GetKey(KeyCode.Z)){
                zoom(-zoomFactor);
            }
            if(Input.GetKey(KeyCode.S)){
                zoom(zoomFactor);
            }
            if(Input.GetKey(KeyCode.UpArrow)){
                rotate(new Vector2(0,-1f));
            }
            if(Input.GetKey(KeyCode.DownArrow)){
                rotate(new Vector2(0,1f));
            }
            if(Input.GetKey(KeyCode.LeftArrow)){
                rotate(new Vector2(-1f,0));

            }
            if(Input.GetKey(KeyCode.RightArrow)){
                rotate(new Vector2(1f,0));
            }

            // open radial menu
            if(Input.GetKeyDown(KeyCode.M)){
                radialMenu.SetActive(!radialMenu.activeSelf);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                StartCoroutine(TakeSnapshot(true));
            }
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            menuLoader.loadMenu();
        }


    }

    void rotate(Vector2 joystick){
            transform.RotateAround(loadedModel.transform.position, transform.right, joystick.y);
            transform.RotateAround(loadedModel.transform.position, transform.up, joystick.x);
    }

    void zoom(float z){
        Debug.Log(z);
        if (cam.orthographic)
        {
            if ( (z < 0 && (cam.orthographicSize-(z*5)) >= 0.001f) || ( z > 0 && (cam.orthographicSize+(z*5))<= 30))
            {
                cam.orthographicSize += z*5;
            }
        }
        else
        {
            var dist = Vector3.Distance(cam.transform.position, loadedModel.transform.position);
           // Debug.Log(dist);
            if ((z < 0 && dist > 0.15f) || (z > 0 && dist < 10))
            {
                transform.Translate(Vector3.back * z);
            }
        }
    }

    WaitForEndOfFrame frameEnd = new WaitForEndOfFrame();
    WaitForSeconds delaySceneLoading = new WaitForSeconds(3);

    public IEnumerator TakeSnapshot(bool rewrite)
    {
        string path = Application.persistentDataPath + "/" + modelName + ".png";
        if (!FileBrowserHelpers.FileExists(path) || rewrite){
            yield return delaySceneLoading;
            yield return frameEnd;

            virtualPhoto.ReadPixels(new Rect(0, Screen.height/2 - sqr/2, sqr, sqr), 0, 0);
            virtualPhoto.Apply();
            byte[] bytes = virtualPhoto.EncodeToPNG();
            System.IO.File.WriteAllBytes(path, bytes);
            //Debug.Log("saved to " + path);
        }
    }

    int oldButtonValue = 0;
    int oldTriggerValue = 0;

    public void receiveData(string [] input){
        string toLog="";
        //Debug.Log(input[0]);
        int joystickX = Int32.Parse(input[0]);
        int joystickY = Int32.Parse(input[1]);
        int triggerValue = Int32.Parse(input[3]);
        int buttonValue = Int32.Parse(input[2]);

        if (buttonValue-oldButtonValue == 1){
            radialMenu.SetActive(!radialMenu.activeSelf);
            toLog="Button pressed :"+radialMenu.activeSelf + DateTime.Now;
        }
        if (!radialMenu.activeSelf){
            if (triggerValue == 1){
                if (joystickY > 600){
                    isZoom=1;
                    //zoom(zoomFactor);
                    toLog = "Zoom : " + isZoom;
                }
                else if (joystickY < 400){
                    isZoom = -1;
                    //zoom(zoomFactor);
                    toLog = "Zoom : " + isZoom;
                }
                else{
                    isZoom=0;
                }
            }else{
                isZoom=0;
                if (joystickY > 600){
                    rotate(new Vector2(0,-5f));
                }
                if (joystickY < 400){
                    rotate(new Vector2(0,5f));
                }
                if (joystickX > 600){
                    rotate(new Vector2(-5f,0));
                }
                if (joystickX < 400){
                    rotate(new Vector2(5f,0));
                }
                toLog = "Rotate : X " + joystickX + ", Y " + joystickY;
            }

        } else {
            radialMenuScript.changeJoyValue(new Vector2(joystickX, joystickY));
            toLog = "Radial Menu : X "+joystickX + ", Y " + joystickY;
            if (triggerValue-oldTriggerValue == 1){
                //select value
                radialMenuScript.elements[radialMenuScript.index].button.onClick.Invoke();
                toLog = "Radial Select : X "+joystickX + ", Y " + joystickY;
                radialMenu.SetActive(!radialMenu.activeSelf);
                //do math to figure out which value was selected, or print this from radial script
            }
        }
        oldButtonValue = buttonValue;
        oldTriggerValue = triggerValue;
        if(toLog != "") LogManager.writeToLog(toLog + " - "+ DateTime.Now);
        

    }
}
