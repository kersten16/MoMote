using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleFileBrowser;

public class modelViewer : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera cam;
    public GameObject loadedModel;
    public GameObject radialMenu;
    Bounds bounds;
    bool loaded = false;
    public float zoomFactor = 0.001f;

    Texture2D virtualPhoto;
    private string modelName;
    private int sqr = 256;


    void Start()
    {
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
        Debug.Log(bounds);
        float cameraDistance = 0.50f; // Constant factor
        Vector3 objectSizes = bounds.max - bounds.min;
        float objectSize = Mathf.Max(objectSizes.x, objectSizes.y, objectSizes.z);
        Debug.Log(objectSize);
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
        transform.position = Vector3.zero;
        if (loaded){
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

    }

    void rotate(Vector2 joystick){
            transform.RotateAround(loadedModel.transform.position, transform.right, joystick.y);
            transform.RotateAround(loadedModel.transform.position, transform.up, joystick.x);
    }

    void zoom(float z){
        Debug.Log(cam.orthographic);
        if (cam.orthographic)
        {
            if ( (z < 0 && cam.orthographicSize > 0.001f) || ( z > 0 && cam.orthographicSize < 1))
            {
                cam.orthographicSize += z;
            }
        }
        else
        {
            var dist = Vector3.Distance(cam.transform.position, loadedModel.transform.position);
            Debug.Log(dist);
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
            Debug.Log("saved to " + path);
        }
    }
}
