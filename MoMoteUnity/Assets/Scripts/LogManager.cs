using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogManager : MonoBehaviour
{
    private static string FILE_PATH;

    static StreamWriter writer;
    // Start is called before the first frame update
    void Awake(){
        FILE_PATH = Application.persistentDataPath+"/test_log.txt";//+participantID
        //sr = File.CreateText(FILE_PATH);
    }
    void Start()
    {
        writer = new StreamWriter(FILE_PATH, true);
        writer.WriteLine("App Launched " +DateTime.Now);
        writer.Close();
        
    }

    // Update is called once per frame
    public static void writeToLog(string message)
    {
        writer = new StreamWriter(FILE_PATH, true);
        writer.WriteLine(message);
        writer.Close();
    }
}
