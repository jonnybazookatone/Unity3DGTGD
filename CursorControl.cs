using UnityEngine;
using System.Collections;

/// <summary>
/// This script is attached to the player and it controls
/// whether the cursor is locked or unlocked. 
/// 
/// This script accesses the MultiplayerScript
/// 
/// All code is taken from Gamer To Game Developer Series 1
/// http://www.gamertogamedeveloper.com
/// No credit is taken myself
/// </summary>

public class CursorControl : MonoBehaviour {

	// Variables start__________________________
	private GameObject multiplayerManager;
	private MultiplayerScript multiScript;
	
	// Variables end____________________________

	// Use this for initialization
	void Start () 
	{
		if(networkView.isMine == true)
		{
			multiplayerManager = GameObject.Find("MultiplayerManager");
			multiScript = multiplayerManager.GetComponent<MultiplayerScript>();
		}
		else
		{
			enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(multiScript.showDisconnectWindow == false)
		{
			Screen.lockCursor = true;
		}

		if(multiScript.showDisconnectWindow == true)
		{
			Screen.lockCursor = false;
		}
	
	}
}
