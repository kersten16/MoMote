using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExpeLoader : MonoBehaviour
{
    public Participant participant;
    public ArduinoInput arduinoInput;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(participant.Trials[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadExpe(){
        StartCoroutine(LoadScene("ExpeViewer", "ExpeMenu"));
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
        
        GameObject newSceneCam = SceneManager.GetActiveScene().GetRootGameObjects()[0];
        newSceneCam.GetComponent<ExpeManager>().participant = participant;
        arduinoInput.messageListener = newSceneCam;

        SceneManager.UnloadSceneAsync(unload);
    }
    
}
