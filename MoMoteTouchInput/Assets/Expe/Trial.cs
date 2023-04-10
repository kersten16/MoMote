using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trial
{
    public int TrialID { get; set; }
    public int ParticipantID { get; set; }
    public int Block1 { get; set; }
    public int Block2 { get; set; }
    public string F { get; set; }
    public string Z { get; set; }

    public Trial (int TrialID, int ParticipantID, int Block1, int Block2, string F, string Z){
        this.TrialID = TrialID;
        this.ParticipantID = ParticipantID;
        this.Block1 = Block1;
        this.Block2 = Block2;
        this.F = F;
        this.Z = Z;

    }
}
