using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using CsvHelper;
using System;
using System.Globalization;
// using Participant;
// using Trial;

public class MenuCreator : MonoBehaviour
{
    //public string modelPath;
    public List<Participant> ParticipantList= new List<Participant>();
    public int numTrials = 15;
   // public pointerScroller ps;
    public GameObject content;
	public GameObject menuItemPrefab;

    // Start is called before the first frame update
    void Start()
    {
		TextAsset dataset = Resources.Load("experiment") as TextAsset;
		Debug.Log(dataset);
		var text = dataset.text;
		Debug.Log(text);
		// Debug.Log(lines);
		parseCSV(text.Split('\n'));
		setupMenu(ParticipantList);
		// TextAsset file = Resources.Load<TextAsset>("experiment.csv");
		// if (file != null){
		// 		using(var reader = new StreamReader(new file))
		// 		using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)){
		// 		List<Trial> training = new List<Trial> (){new Trial(1,1,0,"Right","Far")};
		// 		var trainParticipant =new Participant(0, training);
		// 		ParticipantList.Add(trainParticipant);
		// 		var records = csv.GetRecords<Trial>().ToList();
		// 		Debug.Log ( records[0]);
		// 		for (int i = 1; i<= records.Count/numTrials; i++){
		// 			ParticipantList.Add(new Participant(i,records.GetRange(numTrials*(i-1),numTrials)));
		// 		}
		// 	}
		// }
		//string pathFile = Application.streamingAssetsPath + "/experiment.csv";
		// StartCoroutine(getPath(pathFile));
        // using (var reader = new StreamReader(@"Assets/Resources/experiment.csv"))
        // using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        // {
        //     List<Trial> training = new List<Trial> (){new Trial(1,1,0,"Right","Far")};
        //     var trainParticipant =new Participant(0, training);
        //     ParticipantList.Add(trainParticipant);
        //     var records = csv.GetRecords<Trial>().ToList();
		// 	Debug.Log ( records[0]);
        //     for (int i = 1; i<= records.Count/numTrials; i++){
        //         ParticipantList.Add(new Participant(i,records.GetRange(numTrials*(i-1),numTrials)));
        //     }
        // }
		// List<Trial> training = new List<Trial> (){new Trial(1,1,0,"Right","Far")};
		// var trainParticipant =new Participant(0, training);
		// ParticipantList.Add(trainParticipant);
		// for (int i = 1; i<= 15; i++){
		// 	ParticipantList.Add(new Participant(i,training));
		// }
        
    }
        // read from csv for participant
        //generate object for each participant
        //generate training one no matter what

	// public void getFile(){

		
	// 	TextAsset level = Resources.Load<TextAsset>("Levels/" + levelName);
	// 	if(level != null)
	// 	{
	// 		using(StreamReader sr = new StreamReader(new MemoryStream(level.bytes)))
	// 		{
	// 			...
	// 		}
	// 	}
	// }

    public void setupMenu(List<Participant> ParticipantList){
		foreach (var participant in ParticipantList)
		{
			GameObject childObject = Instantiate(menuItemPrefab) as GameObject;
			//childObject.GetComponent<modelLoader>().modelPath = modelPath;
			childObject.transform.SetParent(content.transform);
			childObject.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
			Debug.Log(participant.Trials[0].TrialID);

			childObject.GetComponent<ExpeLoader>().participant = participant;
			//childObject.GetComponent<modelLoader>().modelPreview.overrideSprite = Resources.Load<Sprite>(Application.persistentDataPath + "/" + menuItem.name.Split('.')[0] + ".png");
			//childObject.GetComponent<modelLoader>().modelPreview.overrideSprite = LoadNewSprite(Application.persistentDataPath + "/" + "something.png");
			//ps.selectObj();
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


	void parseCSV(string[] lines) {
		var lists = new List<List<string>>();
		var columns = 0;
		for(int i = 0; i < lines.Length; i++) {
			if(string.IsNullOrWhiteSpace(lines[i])) continue;
			var data = lines[i].Split(',');
			var list = new List<string>(data);
			lists.Add(list);
			columns = Mathf.Max(columns, data.Length);
		}
		
		var rows = lists.Count;
		var lastParticipant = 0;
		var trials = new List<Trial>();
		

		for(int i = 1; i < rows; i++) {
			//if (lists[i][0] == "Phone"){
				if (Int32.Parse(lists[i][1]) != lastParticipant && Int32.Parse(lists[i][1]) != 1){
					ParticipantList.Add(new Participant(Int32.Parse(lists[i][1]), trials));
					trials.Clear();
					lastParticipant++;
				}
				trials.Add(new Trial(Int32.Parse(lists[i][2]), Int32.Parse(lists[i][1]), Int32.Parse(lists[i][3]), lists[i][4], lists[i][5]));
			//}
		}
	}

}


