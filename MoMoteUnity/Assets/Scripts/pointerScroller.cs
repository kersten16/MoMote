using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class pointerScroller : MonoBehaviour
{
    public GameObject Content;
    public EventSystem System;
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
            System.SetSelectedGameObject(Content.transform.GetChild(count-1).gameObject);
            //Debug.Log("selected 0");
        } else if (value <= 0){
            System.SetSelectedGameObject(Content.transform.GetChild(0).gameObject);
            //Debug.Log("selected last");
        } else {
            int window = 100/(count);
            for (int i = 0; i < count; i++){
                if (value*100 <  window*(i+1) && value*100 >= window*i){
                    System.SetSelectedGameObject(Content.transform.GetChild(i).gameObject);
                    //Debug.Log("selected " + (i));
                }
            }
        }
        //Debug.Log(this.gameObject.GetComponent<Scrollbar>().value);
        //if (this.gameObject.getCompoent<Scrollbar>().value)
    }
    
}
