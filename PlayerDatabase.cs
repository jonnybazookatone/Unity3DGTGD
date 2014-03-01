using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This script manages the PlayerList.
/// 
/// This script is attached to the GameManager
/// 
/// This script is accessed by the PlayerName script
/// 
/// This script is accessed by the HealthAndDamage script
/// 
/// All code is taken from Gamer To Game Developer Series 1
/// http://www.gamertogamedeveloper.com
/// No credit is taken myself
/// </summary>

public class PlayerDatabase : MonoBehaviour {

	// Variables start_______________________
	public List<PlayerDataClass> PlayerList = new List<PlayerDataClass>();

	// This is used to add the player to the list in the first place
	public NetworkPlayer networkPlayer;

	// These are used to update the player list with the name of the player
	public bool nameSet = false;
	public string playerName;

	// These are used to update the player list with the score of the player
	public bool scored = false;
	public int playerScore;

	// These are used to update the player list with the player's team
	public bool joinedTeam = false;
	public string playerTeam;
	// Variables end_________________________


	// Use this for initialization
	void Start () {
	
	}


	// Update is called once per frame
	void Update () 
	{
		if(nameSet == true)
		{
			// Edit the player's record in the list and add their name
			networkView.RPC("EditPlayerListWithName", RPCMode.AllBuffered,
			                Network.player, playerName);
			nameSet = false;
		}

		if(scored == true)
		{
			// Edit the player's score in their record in the list
			networkView.RPC("EditPlayerListWithScore", RPCMode.AllBuffered,
			                Network.player, playerScore);
			scored = false;
		}

		if(joinedTeam == true)
		{
			// Edit the player's record to include the team they're on
			networkView.RPC("EditPlayerListWithTeam", RPCMode.AllBuffered,
			                Network.player, playerTeam);
			joinedTeam = false;
		}

	}


	void OnPlayerConnected (NetworkPlayer netPlayer)
	{
		// Add the player to the list. This is executed on the server
		networkView.RPC("AddPlayerToLst", RPCMode.AllBuffered, netPlayer);
	}


	void OnPlayerDisconnectd (NetworkPlayer netPlayer)
	{
		// Remove the player from the list. This is executed on the server
		networkView.RPC("RemovePlayerFromList", RPCMode.AllBuffered, netPlayer);
	}

	[RPC]
	void AddPlayerToList (NetworkPlayer nPlayer)
	{
		// Create a new entry in the PlayerList and supply the player's network ID
		// as the first entry

		PlayerDataClass capture = new PlayerDataClass();
		capture.networkPlayer = int.Parse(nPlayer.ToString());
		PlayerList.Add(capture);
	}

	[RPC]
	void RemovePlayerFromList (NetworkPlayer nPlayer)
	{
		// Find the player in the player list based on their
		// networkplayer ID and then remove them
		for(int i = 0; i < PlayerList.Count; i++)
		{
			if(PlayerList[i].networkPlayer == int.Parse(nPlayer.ToString))
			{
				PlayerList.RemoveAt(i);
			}
		}
	}

	[RPC]
	void EditPlayerListWithName (NetworkPlayer nPlayer, string pName)
	{
		// Find the player in the player list based on their
		// networkplayer ID and then add their name to the list
		for(int i = 0; i < PlayerList.Count; i++)
		{
			if(PlayerList[i].networkPlayer == int.Parse(nPlayer.ToString))
			{
				PlayerList[i].playerName = pName;
			}
		}
	}

	[RPC]
	void EditPlayerListWithScore (NetworkPlayer nPlayer, int pScore)
	{
		// Find the player in the player list based on their
		// networkplayer ID and then edit their score to the list
		for(int i = 0; i < PlayerList.Count; i++)
		{
			if(PlayerList[i].networkPlayer == int.Parse(nPlayer.ToString))
			{
				PlayerList[i].playerScore = pScore;
			}
		}
	}

	[RPC]
	void EditPlayerListWithTeam (NetworkPlayer nPlayer, string pTeam)
	{
		// Find the player in the player list based on their
		// networkplayer ID and then add their team to the list
		for(int i = 0; i < PlayerList.Count; i++)
		{
			if(PlayerList[i].networkPlayer == int.Parse(nPlayer.ToString))
			{
				PlayerList[i].playerTeam = pTeam;
			}
		}
	}

	// http://www.youtube.com/watch?v=NV8qoQBVPH8 45:33
}
