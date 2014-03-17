using UnityEngine;
using System.Collections;

/// <summary>
/// This script incorporates the player's name into the game
/// 
/// This script accesses the PlayerDatabase script to tell it to
/// add the player's name to the PlayerList
/// 
/// This script accesses the PlayerLabel script to supply it with
/// the player's name
/// 
/// All code is taken from Gamer To Game Developer Series 1
/// http://www.gamertogamedeveloper.com
/// No credit is taken myself
/// </summary>

public class PlayerName : MonoBehaviour {

	// Variables start___________________
	public string playerName;
	// Variables end_____________________


	// This will run before the Start or Update Unity commands
	void Awake() {
		// When the player spawns into the game retrieve their name
		// from the PlayerPrefs and ensure that this name is not any
		// other players name

		if(networkView.isMine == true)
		{
			playerName = PlayerPrefs.GetString("playerName");

			// Check if any players in the game already have the same
			// name as this player and if they do then assign a random
			// number as their name and save it
			foreach(GameObject objNameCheck in GameObject.FindObjectsOfType(typeof(GameObject)))
			{
				if(playerName == objNameCheck.name)
				{
					float x = Random.Range(0, 1000);
					playerName = "(" + x.ToString() + ")";
					PlayerPrefs.SetString("playerName", playerName);
				}
			}

			// Update the local GameManager with the player's name so that
			// their name is appended to the PlayerList
			UpdateLocalGameManager(playerName);

			// Send out an RPC to ensure this player's name is slapped into
			// their GameObject accross the network.
			networkView.RPC ("UpdateMyNameEverywhere", RPCMode.AllBuffered, playerName);
		}
	}
	

	void UpdateLocalGameManager (string pName)
	{
		// Tell the PlayerDatabase script to append this
		// player's name to the list.
		GameObject gameManager = GameObject.Find("GameManager");

		PlayerDatabase dataScript = gameManager.GetComponent<PlayerDatabase>();
		dataScript.nameSet = true;
		dataScript.playerName = pName;
	}


	[RPC]
	void UpdateMyNameEverywhere (string pName)
	{
		// Change the player's GameObject name to their actual
		// player name
		gameObject.name = pName;
		playerName = pName;

		// Supply the PlayerLabel script with the player's name
		PlayerLabel labelScript = transform.GetComponent<PlayerLabel>();
		labelScript.playerName = pName;
	}
}
