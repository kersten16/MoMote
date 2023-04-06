using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using CsvHelper;

public class MenuCreator : MonoBehaviour
{
    public string modelPath;
    public List<Participant> ParticipantList;
    public int numTrials = 15;
    // Start is called before the first frame update
    void Start()
    {
        using(var reader = new StreamReader(@"C:\Users\Kersten\Desktop\designProject\MoMote\MoMoteUnity\Assets\Resources\experiment.csv"))
    {
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            Trial training = New Trial(1,0,1, "Right", "Far");
            ParticipantList.Add(New Participant(0,training));
            var records = csv.GetRecords<Trial>();
            for (int i = 1; i<= records.Length; i++){
                ParticipantList.Add(New Participant(i,records.GetRange(numTrials*(i-1),numTrials)));
            }

        }
    }
        // read from csv for participant
        //generate object for each participant
        //generate training one no matter what
        
    }

    public void setupMenu(List<Participant> ParticipantList){
		foreach (var participant in ParticipantList)
		{
			GameObject childObject = Instantiate(menuItemPrefab) as GameObject;
			//childObject.GetComponent<modelLoader>().modelPath = modelPath;
			childObject.GetComponent<modelLoader>().modelName = "Participant " + participant.ID;//participant number;
			childObject.transform.SetParent(content.transform);
			childObject.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
			childObject.GetComponent<modelLoader>().arduinoInput = arduinoInput;
			//childObject.GetComponent<modelLoader>().modelPreview.overrideSprite = Resources.Load<Sprite>(Application.persistentDataPath + "/" + menuItem.name.Split('.')[0] + ".png");
			childObject.GetComponent<modelLoader>().modelPreview.overrideSprite = LoadNewSprite(Application.persistentDataPath + "/" + "something.png");
			ps.selectObj();
		}
	}
}

public class Trial
{
    public Trial ( int TrialID , int ParticipantID, int Block1 ,string F, string IV){
        this.TrialID = TrialID;
        this.ParticipantID =ParticipantID;
        this.Block1 = Block1;
        this.F =F;
        this.IV= IV;
    }
    public int TrialID { get; set; }
    public int ParticipantID { get; set; }
    public int Block1 { get; set; }
    public string F { get; set; }
    public string IV { get; set; }
}

public class Participant
{
    public int ID { get; set; }
    public Trial[] Trials { get; set; }

    public Participant ( int ID, Trial[] trialset){
        this.ID = ID
        this.Trials = trialset
    }
}


