using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class modelLoader : MonoBehaviour
{
    public string modelName;
    public string modelPath;

    public GameObject modelTextMesh;

    // Start is called before the first frame update
    void Start()
    {
        // set Model Name game object text
        modelTextMesh.GetComponent<TextMeshProUGUI>().SetText(modelName);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void loadModel(){
        // open scene with model from path
    }
}
