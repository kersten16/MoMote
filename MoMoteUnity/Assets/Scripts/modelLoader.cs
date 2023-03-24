using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Dummiesman;
using System.IO;

public class modelLoader : MonoBehaviour
{
    public string modelName;
    public string modelPath;

    public GameObject modelTextMesh;
    private GameObject loadedModel;

    // Start is called before the first frame update
    void Start()
    {
        // set Model Name game object text
        modelTextMesh.GetComponent<TextMeshProUGUI>().SetText(modelName);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Active Scene : " + SceneManager.GetActiveScene().name);
    }

    public void loadModel(){
        // open scene with model from path
        //SceneManager.LoadScene("ModelViewer");
        StartCoroutine(LoadScene("ModelViewer", "MenuScene"));
        
    }
    IEnumerator LoadScene(string name, string unload)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            Debug.Log("Loading scene " + " [][] Progress: " + asyncLoad.progress);
            yield return null;
        }

        //Activate the Scene
        asyncLoad.allowSceneActivation = true;
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));

        if (!File.Exists(modelPath))
        {
            Debug.LogError("Please set FilePath in ObjFromFile.cs to a valid path.");
            yield break;
        }

        //load
        var loader = new OBJLoader();
        loadedModel = loader.Load(modelPath);
        Debug.Log(loadedModel);
        GameObject newSceneCam = SceneManager.GetActiveScene().GetRootGameObjects()[0];
        Debug.Log(newSceneCam.gameObject.name);
        newSceneCam.GetComponent<modelViewer>().loadedModel = loadedModel;
        newSceneCam.GetComponent<modelViewer>().setCam();
        // todo set the camera to be close to the object

        SceneManager.UnloadSceneAsync(unload);
    }
}
