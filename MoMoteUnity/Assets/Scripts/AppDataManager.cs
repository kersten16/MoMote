using System.IO;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using SimpleFileBrowser;
using UnityEngine;

public class AppDataManager : MonoBehaviour
{
    // Create a field of this class for the file.
    string saveFile;
    List<AppData> appData = new List<AppData>(); //can create our own class that has properties needed for the list

    void Awake()
    {
        // Update the field once the persistent path exists.
        //saveFile = Application.persistentDataPath + "/appData.json";
        saveFile = "./Assets/Resources/appData.json";
    }

    public List<AppData> readFile()
    {
        // Does the file exist?
        if (File.Exists(saveFile))
        {
            // Work with JSON
            string fileContents = File.ReadAllText(saveFile);
            AppData [] _tempData = JsonHelper.FromJson<AppData>(fileContents);
            appData = _tempData.OfType<AppData>().ToList();

        }
        return appData;
    }

    public void writeFile(string[] addedFiles)
    {
        for( int i = 0; i <addedFiles.Length; i++ ){
            Debug.Log(addedFiles[i]);
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

 public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Item;
        }
 
        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Item = array;
            return JsonUtility.ToJson(wrapper);
        }
 
        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Item = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }
 
        [Serializable]
        private class Wrapper<T>
        {
            public T[] Item;
        }
    }