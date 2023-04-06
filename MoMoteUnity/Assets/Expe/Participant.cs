using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Participant
{
    public int ID { get; set; }
    public Trial[] Trials { get; set; }

    public Participant (int ID,Trial[] Trials) {
        this.ID = ID;
        this.Trials = Trials;
    }

}
