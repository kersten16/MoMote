using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using CsvHelper;
// using Participant;
// using Trial;

public class MenuCreator : MonoBehaviour
{
    public string modelPath;
    public List<Participant> ParticipantList;
    public int numTrials = 15;
    // Start is called before the first frame update
    void Start()
    {
        using (var reader = new StreamReader(@"C:\Users\Kersten\Desktop\designProject\MoMote\MoMoteUnity\Assets\Resources\experiment.csv"))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            List<Trial> training = {new Trial(1,1,0,"Right","Far")};
            var trainParticipant =new Participant(0, training);
            ParticipantList.Add(trainParticipant);
            var records = csv.GetRecords<Trial>();
            for (int i = 1; i<= records.Length; i++){
                ParticipantList.Add(new Participant(i,records.GetRange(numTrials*(i-1),numTrials)));
            }
        }
        
    }
        // read from csv for participant
        //generate object for each participant
        //generate training one no matter what

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


