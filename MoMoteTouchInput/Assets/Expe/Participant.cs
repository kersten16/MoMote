using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Participant
{
    public int ID { get; set; }
    public List<Trial> Trials { get; set; }

    public Participant (int ID,List<Trial> Trials) {
        this.ID = ID;
        this.Trials = Trials;
    }

}
