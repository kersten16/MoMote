using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExpeManager : MonoBehaviour
{
    public Participant participant;

    public Camera cam;
    public GameObject camParent;
    public modelViewer modelViewer;
    public GameObject finalMessage;
    public GameObject radialMenu;

    public GameObject mainMenuButton;

    public GameObject cube;
    public TextMeshPro Left;
    public TextMeshPro Right;
    public TextMeshPro Bottom;
    public TextMeshPro Top;
    public TextMeshPro Back;

    string letter;
    string face;
    int trialNb = 0;
    int errorNb = 0;
    long startTime;
    bool running = false;

    public List<Trial> trials = new List<Trial>();

    Trial currentTrial;

    string[] Letters = new string[]{"A","B","C","D"};
    
    string currentLetter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setExpe(){
        Debug.Log(participant.Trials[0].ParticipantID);
        if (participant.ID != 0){
            mainMenuButton.SetActive(false);
        }
        trials = participant.Trials;
        
        currentTrial = trials[0];

        // init
        resetCubeTransform();
        setOrthoSize(currentTrial.Z);
        currentLetter = Letters[UnityEngine.Random.Range(0, 4)];
        setLetterOnCurrFace(currentLetter);
        startTime = DateTime.Now.Ticks;
        modelViewer.loaded = true;
        running = true;
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void setLetterOnCurrFace(string l){
        // reset texts
        Left.SetText("");
        Right.SetText("");
        Top.SetText("");
        Bottom.SetText("");
        Back.SetText("");
        Debug.Log("setting letter to " + l);

        switch (currentTrial.F)
        {
            case "Left":
                Left.SetText(l);
                break;
            case "Right":
                Right.SetText(l);
                break;
            case "Top":
                Top.SetText(l);
                break;
            case "Bottom":
                Bottom.SetText(l);
                break;
            case "Back":
                Back.SetText(l);
                break;
            default:
                break;
        }
    }

    public void buttonCLicked(string name){
        radialMenu.SetActive(!radialMenu.activeSelf);
        Debug.Log("test I pressed a button lol");
        if (name == currentLetter){
            if (running){
                long finalTime = DateTime.Now.Ticks-startTime;
                LogManager.writeToCsv("Momote," + currentTrial.TrialID + "," + currentTrial.ParticipantID + "," + currentTrial.Block1 + "," + currentTrial.Block2 + ",Phone," + currentTrial.F + "," + currentTrial.Z + "," + finalTime + "," + errorNb);
            }

            // log trial info & Time.time - startTime & errorNb
            Debug.Log("trial success");
            nextTrial();
        } else {
            Debug.Log("trial failed");
            errorNb++;
            // reset trial with a new letter
            resetCubeTransform();
            setOrthoSize(currentTrial.Z);
            currentLetter = Letters[UnityEngine.Random.Range(0, 4)];
            setLetterOnCurrFace(currentLetter);
        }
    }

    // on click check if option is the "solution" if yes go to next trial, if no start it again

    void nextTrial(){
        trialNb++;
        if (trialNb < trials.Count){
            errorNb = 0;
            // set text of current face to ""
            currentTrial = trials[trialNb];
            currentLetter = Letters[UnityEngine.Random.Range(0, 4)];
            // set text of current face to random letter
            resetCubeTransform();
            setOrthoSize(currentTrial.Z);
            setLetterOnCurrFace(currentLetter);
            startTime = DateTime.Now.Ticks;
        } else {
            endTrials();
        }
    }

    void endTrials(){
        running = false;
        modelViewer.loaded = false;
        finalMessage.SetActive(true);
    }

    void setOrthoSize(string z){
        float zoom = 12;
        if (z == "Close"){
            zoom = 0.1f;
        } else if(z == "Normal"){
            zoom = 12;
        } else if (z == "Far"){
            zoom = 30;
        }
        cam.orthographicSize = zoom;
    }

    void resetCubeTransform(){
        cube.transform.position = Vector3.zero;
        camParent.transform.eulerAngles = Vector3.zero;
    }

    // on menu, open to define the participant
}
