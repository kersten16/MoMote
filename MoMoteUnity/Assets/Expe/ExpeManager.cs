using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExpeManager : MonoBehaviour
{
    public int participant;

    public Camera cam;
    public RMF_RadialMenu radialMenuScript;

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
    float startTime;

    Trial[] trials;

    Trial currentTrial;

    string[] Letters = new string[]{"A","B","C","D"};
    
    string currentLetter;

    // Start is called before the first frame update
    void Start()
    {
        // read csv
        // var fileData = System.IO.File.ReadAllText("../Resources/experiment.csv");
        // var lines = fileData.Split("\n"[0]);
        // //var lineData = (lines[0].Trim()).Split(","[0]);
        // foreach(string line in lines){
        //     var l = line.Split(",");
        //     if (Int32.Parse(l[1]) == participant){
        //         trials.Add(new Trial(l[4],l[5]));
        //     }
        // }
        currentTrial = trials[0];

        // init
        resetCubeTransform();
        setOrthoSize(currentTrial.Z);
        currentLetter = Letters[UnityEngine.Random.Range(0, 4)];
        setLetterOnCurrFace(currentLetter);
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

    public void buttonCLicked(){
        string name = radialMenuScript.elements[radialMenuScript.index].gameObject.name;
        if (name == currentLetter){
            // log trial info & Time.time - startTime & errorNb
            nextTrial();
        } else {
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
        if (trialNb < trials.Length){
            errorNb = 0;
            // set text of current face to ""
            currentTrial = trials[trialNb];
            currentLetter = Letters[UnityEngine.Random.Range(0, 4)];
            // set text of current face to random letter
            resetCubeTransform();
            setOrthoSize(currentTrial.Z);
            setLetterOnCurrFace(currentLetter);
            startTime = Time.time;
        } else {
            endTrials();
        }
    }

    void endTrials(){

    }

    void setOrthoSize(string z){
        int zoom = 2;
        if (z == "Close"){
            zoom = 1;
        } else if(z == "Normal"){
            zoom = 2;
        } else if (z == "Far"){
            zoom = 3;
        }
        cam.orthographicSize = zoom;
    }

    void resetCubeTransform(){
        cube.transform.position = Vector3.zero;
        cube.transform.Rotate(-cube.transform.rotation.eulerAngles);
    }

    // on menu, open to define the participant
}
