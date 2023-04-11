using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogManager : MonoBehaviour
{
    private static string FILE_PATH;
    private static string CSV_PATH;
    public static int participantID;

    static StreamWriter logWriter;
    static StreamWriter csvWriter;
    // Start is called before the first frame update
    // void Awake(){
    //     FILE_PATH = "Assets/Logs/input_log"+Time.time+".txt";//+participantID
    //     CSV_PATH = "Assets/Logs/experiment_log"+Time.time+".csv";//+participantID
    //     //sr = File.CreateText(FILE_PATH);
    // }
    public void StartExperiment(int ID)
    {
        participantID = ID;
        if (participantID == 0) return;
        FILE_PATH = Application.persistentDataPath + "/InputLog_"+participantID+"_"+DateTime.Now.ToString("MMMd_HHmm")+".txt";
        CSV_PATH = Application.persistentDataPath + "/ExperimentLog_"+participantID+"_"+DateTime.Now.ToString("MMMd_HHmm")+".csv";
        logWriter = new StreamWriter(FILE_PATH, true);
        logWriter.WriteLine("App Launched " + DateTime.Now);
        logWriter.Close();
        csvWriter = new StreamWriter(CSV_PATH, true);
        csvWriter.WriteLine("DesignName,ParticipantID,TrialID,Block1,F,Z,Time,ErrorCount");
        csvWriter.Close();
    }

    // Update is called once per frame
    public static void writeToLog(string message)
    {
        if (participantID == 0) return;
        logWriter = new StreamWriter(FILE_PATH, true);
        logWriter.WriteLine(message);
        logWriter.Close();
    }

    public static void writeToCsv(string message){
        if (participantID == 0) return;
        csvWriter = new StreamWriter(CSV_PATH, true);
        csvWriter.WriteLine(message);
        Debug.Log("logged "+message);
        csvWriter.Close();
    }
}
