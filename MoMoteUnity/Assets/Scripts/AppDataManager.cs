using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppDataManager : MonoBehaviour
{
    // Create a field of this class for the file.
    string saveFile;
    List<AppData> appData = new List<AppData>(); //can create our own class that has properties needed for the list

    void Awake()
    {
        // Update the field once the persistent path exists.
        saveFile = Application.persistentDataPath + "/appdata.json";
    }

    public void readFile()
    {
        // Does the file exist?
        if (File.Exists(saveFile))
        {
            // Work with JSON
            string fileContents = File.ReadAllText(saveFile);
            AppData [] _tempData = JsonHelper.FromJson<AppData>(fileContents);
            appData = _tempData.OfType<AppData>().ToList();
        }
    }

    public void writeFile(List<String> addedFiles)
    {
        for( int i = 0; i <addedFiles.Length; i++ ){
            AppData newEntry = new AppData();
            newEntry.path= addedFiles[i];
			newEntry.name = FileBrowserHelpers.GetFilename(addedFiles[i]);
            appData.Add(newEntry);
        }
        string jsonString = JsonHelper.ToJson(appData.ToArray());
        // Work with JSON
        File.WriteAllText(saveFile, jsonString);
    }
}