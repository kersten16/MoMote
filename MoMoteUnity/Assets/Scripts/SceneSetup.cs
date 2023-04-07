using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSetup : MonoBehaviour
{
    public GameObject listener;
    // Start is called before the first frame update
    void Start()
    {
        var receiver = GameObject.Find("ArduinoReceiver");
        receiver.GetComponent<ArduinoInput>().messageListener = listener;  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
