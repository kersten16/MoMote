using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trial
{
    public int TrialID { get; set; }
    public int ParticipantID { get; set; }
    public int Block1 { get; set; }
    public string F { get; set; }
    public string IV { get; set; }

    public Trial (int TrialID, int ParticipantID, int Block1, string F, string IV){
        this.TrialID = TrialID;
        this.ParticipantID = ParticipantID;
        this.Block1 = Block1;
        this.F = F;
        this.IV = IV;

    }
}
