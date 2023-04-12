using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using CsvHelper;
using System.Globalization;
// using Participant;
// using Trial;

public class MenuCreator : MonoBehaviour
{
    public string modelPath;
    public List<Participant> ParticipantList= new List<Participant>();
    public int numTrials = 60;
    public pointerScroller ps;
    public GameObject content;
	public GameObject menuItemPrefab;

    // Start is called before the first frame update
    void Start()
    {
        using (var reader = new StreamReader(@"Assets/Resources/MoMote_Experiment.csv"))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
			List<Trial> training = new List<Trial> (){new Trial(1,0,0,0,"MoMote","Right","Far"), new Trial(2,0,0,0,"MoMote","Left","Close"), new Trial(3,0,0,0,"MoMote","Back","Normal")};
            var trainParticipant =new Participant(0, training);
            ParticipantList.Add(trainParticipant);
            var records = csv.GetRecords<Trial>().ToList();
			Debug.Log ( records[1].ParticipantID + " " + records[1].D );
            for (int i = 1; i<= records.Count/numTrials; i++){
				//records.FindAll((t) => t.ParticipantID == i && t.D == "MoMote");
				ParticipantList.Add(new Participant(i,records.GetRange(numTrials*(i-1),numTrials)));
                //ParticipantList.Add(new Participant(i,records.FindAll((t) => t.ParticipantID == i && t.D == "MoMote")));
            }
        }
        setupMenu(ParticipantList);
    }
        // read from csv for participant
        //generate object for each participant
        //generate training one no matter what

    public void setupMenu(List<Participant> ParticipantList){
		foreach (var participant in ParticipantList)
		{
			GameObject childObject = Instantiate(menuItemPrefab) as GameObject;
			//childObject.GetComponent<modelLoader>().modelPath = modelPath;
			childObject.transform.SetParent(content.transform);
			childObject.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
			// foreach (var item in participant.Trials)
			// {
			// 	Debug.Log(item.TrialID + " D: " + item.D);
			// }
			// Debug.Log(participant.ID +" Before remove "+participant.Trials[0].D);
			participant.Trials.RemoveAll((t)=> t.D == "Phone");
			Debug.Log(participant.ID +" After remove "+participant.Trials.Count);
			childObject.GetComponent<ExpeLoader>().participant = participant;
			//childObject.GetComponent<modelLoader>().modelPreview.overrideSprite = Resources.Load<Sprite>(Application.persistentDataPath + "/" + menuItem.name.Split('.')[0] + ".png");
			//childObject.GetComponent<modelLoader>().modelPreview.overrideSprite = LoadNewSprite(Application.persistentDataPath + "/" + "something.png");
			ps.selectObj();
		}
	}

    	public Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f)
	{

		// Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference

		Texture2D SpriteTexture = LoadTexture(FilePath);
		Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit);

		return NewSprite;
	}

    public Texture2D LoadTexture(string FilePath)
	{

		// Load a PNG or JPG file from disk to a Texture2D
		// Returns null if load fails

		Texture2D Tex2D;
		byte[] FileData;

		if (File.Exists(FilePath))
		{
			FileData = File.ReadAllBytes(FilePath);
			Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
			if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
				return Tex2D;                 // If data = readable -> return texture
		}
		return null;                     // Return null if load failed
	}

}


