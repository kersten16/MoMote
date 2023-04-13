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
        this.Trials = new List<Trial>();
        foreach(Trial trial in Trials){
            this.Trials.Add(new Trial(trial.TrialID, trial.ParticipantID, trial.Block1, trial.Block2, trial.D, trial.F, trial.Z));
        }
    }

}
