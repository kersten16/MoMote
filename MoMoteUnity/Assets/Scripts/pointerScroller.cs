using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class pointerScroller : MonoBehaviour
{
    public GameObject Content;
    public ScrollRect ScrollView;
    public EventSystem ESystem;
    public ArduinoInput arduinoInput;
    // Start is called before the first frame update
    void Start()
    {  
        //System.SetSelectedGameObject(Content.transform.GetChild(Content.transform.childCount-1).gameObject);
        //selectObj();
    }

    // Update is called once per frame
    void Update()
    {}

    public void selectObj(){
        int count = Content.transform.childCount;
        float value = this.gameObject.GetComponent<Scrollbar>().value;
        if (value >= 1){
            ESystem.SetSelectedGameObject(Content.transform.GetChild(count-1).gameObject);
            //Debug.Log("selected 0");
        } else if (value <= 0){
            ESystem.SetSelectedGameObject(Content.transform.GetChild(0).gameObject);
            //Debug.Log("selected last");
        } else {
            int window = 100/(count);
            for (int i = 0; i < count; i++){
                if (value*100 <  window*(i+1) && value*100 >= window*i){
                    ESystem.SetSelectedGameObject(Content.transform.GetChild(i).gameObject);
                    //Debug.Log("selected " + (i));
                }
            }
        }
        //Debug.Log(this.gameObject.GetComponent<Scrollbar>().value);
        //if (this.gameObject.getCompoent<Scrollbar>().value)
    }

    private int oldTriggerPress = 0;
    private int currentTriggerPress;

    public void receiveData(string [] input){
        Debug.Log(input);
        currentTriggerPress = Int32.Parse(input[3]);
        //int joystickX = Int32.Parse(input[0]);
        float contentHeight = ScrollView.content.sizeDelta.y;
        int joystickY = Int32.Parse(input[1]);
        if (joystickY > 600){
            ScrollView.verticalNormalizedPosition += (20/contentHeight);
        }
        else if (joystickY < 400){
            ScrollView.verticalNormalizedPosition -= (20/contentHeight);
        }
        if (currentTriggerPress-oldTriggerPress == 1){
            ESystem.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
        }
        oldTriggerPress = currentTriggerPress;
    }
    
}
