using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// A list of gameobjects does not persist through scenes. So we have to store the clue's information
// in a static list of type ClueInfo (a Struct) in order to be able to access it through scenes.
public struct ClueInfo
{
	//public int id;
	//public int rating;
	public string clueName;
	public string description;
	public string roomToSpawnIn;

	// Constructor that passes a ClueItem object by reference
	public ClueInfo (ref ClueItem item)
	{
		//id = item.ID;
		//rating = item.Rating;
		clueName = item.ClueName;
		description = item.Description;
		roomToSpawnIn = item.Room;
	}

	// Copy Constructor for ClueInfo
	public ClueInfo (ref ClueInfo clue)
	{
		//id = clue.id;
		//rating = clue.rating;
		clueName = clue.clueName;
		description = clue.description;
		roomToSpawnIn = clue.roomToSpawnIn;
	}
}

public class ClueItem : MonoBehaviour {

	//public int id;
	//public int rating;
	public string clueName;
	public string description;
	public string room;
	private bool isCollected;

	// copy Constructor
	public ClueItem (ref ClueItem passedClueItem)
	{
		//ID = passedClueItem.id;
		//Rating = passedClueItem.rating;
		ClueName = passedClueItem.clueName;
		Description = passedClueItem.description;
		IsCollected = passedClueItem.isCollected;
	}

	// Default Constructor
	public ClueItem ()
	{
		ClueName = "Default Name";
		Description = "Default Description";
		isCollected = false;
	}

//	public ClueItem (int ident, int rate, string cName, string descrip, bool collected)
//	{
//		ID = ident;
//		Rating = rate;
//		ClueName = cName;
//		Description = descrip;
//		IsCollected = collected;
//	}

	// Using this ClueItem for testing purposes because we may not need an id or rating for now.
	public ClueItem (string cName, string descrip, string room, bool collected)
	{
		ClueName = cName;
		Description = descrip;
		IsCollected = collected;
		Room = room;
	}

	// Properties to get and set all variables
//	public int ID
//	{
//		get { return id; }
//		set { id = value; }
//	}

//	public int Rating
//	{
//		get { return rating; }
//		set { rating = value; }
//	}

	public string Room
	{
		get { return room; }
		set { room = value; }
	}

	public string ClueName
	{
		get { return clueName; }
		set { clueName = value; }
	}

	public string Description
	{
		get { return description; }
		set { description = value; }
	}

	public bool IsCollected
	{
		get { return isCollected; }
		set { isCollected = value; }
	}


	public void OnMouseDown ()
	{
		// Debug message to make sure clicking works
		print ("Entered OnMouseDown of ClueItem");
		ClueItem cI = this.GetComponent<ClueItem> ();
		ClueBuilder.S.DisplayInfo.text = "Name: " + cI.ClueName + "\n"
		+ "Description: " + cI.Description + "\n"
		+ "Room To Spawn In: " + cI.Room + "\n";
	}
}
