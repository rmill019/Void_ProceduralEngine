using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class ClueBuilder : MonoBehaviour {

	public Text					displayInfo;
	public TextAsset			ClueTableJSON;
	public static ClueBuilder	S;

	// Constants that hold the max number of entries per section in the JSON file
	private const int			NUMBER_OF_MODELS = 10;
	private const int 			NUMBER_OF_DESCRIPTIONS = 3;
	private const int 			NUMBER_OF_ROOMS = 7;
	// Constant to hold how many clues we wish to procedurally generate
	private const int			NUMBER_OF_CLUES_TO_GENERATE = 20;

	// List to hold all information of the built clues
	private List<ClueInfo>		_proceduralClues;
	// List to hold the actual GameObjects of assembled Clues. This should be in ClueManager?
	private List<GameObject> 	_assembledClues;

	void Awake ()
	{
		// Assign Singleton
		if (S == null)
		{
			S = this;
		} 
		else if (S != null)
		{
			Destroy (this);
		}
	}

	// Use this for initialization
	void Start () {

		//Delete this as well. The textBox reference should not be in this class
		displayInfo.text = "";

		// Initialize _proceduralClues List
		_proceduralClues = new List<ClueInfo>();

		// For loop with number of clues we wish to generate and then
		// printing each clues information to the console window.
		for (int i = 0; i < NUMBER_OF_CLUES_TO_GENERATE; i++)
		{
			ClueInfo testClue = BuildClueInformation ();
			Debug.Log ("Model Name: " + testClue.clueName + "\n"
				+ "Clue Description: " + testClue.description + "\n"
				+ "Clue Room Location: " + testClue.roomToSpawnIn + "\n\n");
			// Add the built clue to the List of procudurally built clues
			_proceduralClues.Add (testClue);
		}

		// Assemble the clues and store them in _assembledClues
		AssembleClues ();

		// Spawn the generated clues
		SpawnClues ();
	}

	// TODO Delete this. This needs to go somewhere else. It is only here for testing purposes
	public Text DisplayInfo 
	{
		get { return displayInfo; }
		set { displayInfo.text = value.ToString(); }
	}

	public ClueInfo BuildClueInformation ()
	{
		// Create a new ClueItem to hold the information that we will return
		ClueInfo clueToReturn = new ClueInfo ();

		// Create JSON Reader
		JsonData data = JsonMapper.ToObject (ClueTableJSON.text);

		// Pick 3 random numbers, 1 for name, 1 for description, and 1 for room
		int modelNum = Random.Range (0, NUMBER_OF_MODELS);
		int descriptionNum = Random.Range (0, NUMBER_OF_DESCRIPTIONS);
		int roomNum = Random.Range (0, NUMBER_OF_ROOMS);

		// Append the random numbers to a string that we will use to drill down into the JSON file
		string modelNameKey = "name" + modelNum;
		string descriptionKey = "desc" + descriptionNum;
		string roomNameKey = "room" + roomNum;

		// Pick and assign the model name, description and roomName to spawn in from the JSON file;
		clueToReturn.clueName = data["Models"] [0] [modelNameKey].ToString();
		//TODO Delete this comment and debug message below
		// Debug Message to make sure clueName was assigned
		//Debug.Log ("Clue Name: " + clueToReturn.clueName);
		clueToReturn.description = data ["Descriptions"] [0] [clueToReturn.clueName] [0] [descriptionKey].ToString ();
		//TODO Delete this comment and debug message below
		// Debug Message to make sure description was assigned
		//Debug.Log ("Description: " + clueToReturn.description);
		clueToReturn.roomToSpawnIn = data ["Rooms"] [0] [roomNameKey].ToString ();
		//TODO Delete this comment and debug message below
		// Debug Message to make sure room was assigned
		//Debug.Log ("Room to spawn in: " + clueToReturn.roomToSpawnIn);

		// TODO does the Clear function release all resources?
		data.Clear();

		// Return the clue that we have procedurally built from the JSON File values
		return (clueToReturn);
	}


	public void AssembleClues ()
	{
		// Initialize the List of GameObjects that we will use to store the Clues we are about to assemble
		_assembledClues = new List<GameObject> ();

		// Cycle through each ClueInfo that we created and stored in _procedural clues and turn them into actual GameObjects
		foreach (ClueInfo clue in _proceduralClues)
		{
			// Temporary GameObject cloned from prefab in the Resources folder
			GameObject tGO = Instantiate (Resources.Load (clue.clueName, typeof(GameObject)) as GameObject);
			// Add ClueItem Component script to tGO
			tGO.AddComponent <ClueItem> ();
			// Fill out the ClueItem members with the appropriate values we procedurally generated in BuildClueInformation
			tGO.GetComponent <ClueItem> ().ClueName = clue.clueName;
			tGO.GetComponent <ClueItem> ().Description = clue.description;
			tGO.GetComponent <ClueItem> ().Room = clue.roomToSpawnIn;
			// Print Assembled clue information
			print ("tGO name: " + tGO.GetComponent<ClueItem> ().clueName + "\n"
			+ "tGO description: " + tGO.GetComponent<ClueItem> ().description + "\n"
			+ "tGO room: " + tGO.GetComponent<ClueItem> ().room);
			// Add tGO to _assembledClues
			_assembledClues.Add (tGO);
			// Destroy tGO
			Destroy (tGO.gameObject);
		}
	}


	public void SpawnClues ()
	{
		// Spawn and layout the clues so we can Inspect them
		Vector3 pos = new Vector3 (-6, 3, 5);
		for (int i = 0; i < _assembledClues.Count; i++)
		{
			GameObject tGO = Instantiate (_assembledClues [i], pos, Quaternion.identity);
			tGO.name = _assembledClues [i].gameObject.GetComponent<ClueItem>().ClueName;
			pos.x += 4;

			// If we are on 4th clue then move the spawn point down the z-axis and move back to the
			// beginning point on the x-axis
			if (i > 0 && i % 5 == 0)
			{
				pos.x = -6;
				pos.z -= 4;
			}
		}
	}
}
