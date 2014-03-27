using UnityEngine;
using System.Collections;

/// <summary>
/// This script is attached to the GameManager.
/// 
/// This script accesses the SpawnScript to check if firstSpawn is true
/// </summary>

public class AssignHealth : MonoBehaviour {

	// Variables start_____________________

	private GameObject[] redTeamPlayers;
	private GameObject[] blueTeamPlayers;
	private float waitTime = 5;

	// Variables end_______________________

	void OnConnectedToSever ()
	{
		StartCoroutine(AssignHealthOnJoiningGame());
	}

	IEnumerator AssignHealthOnJoiningGame()
	{
		// Do not execute the code until the wait time has elapsed
		yield return new WaitForSeconds(waitTime);

		// Find the Trigger Game Objects of all players in both teams
		// and place a reference to these in the two arrays
		redTeamPlayers = GameObject.FindGameObjectsWithTag("RedTeamTrigger");
		blueTeamPlayers = GameObject.FindGameObjectsWithTag("BlueTeamTrigger");

		// Assign the buffered previousHealth value to the player's current health
		// If we did not do this then a new player joining the game, would have an
		// incorrect value of everyones health, as they would all appear to have
		// full health, even though they do not
		foreach(GameObject red in redTeamPlayers)
		{
			HealthAndDamage HDScript = red.GetComponent<HealthAndDamage>();
			HDScript.myHealth = HDScript.previousHealth;
		}

		foreach(GameObject blue in blueTeamPlayers)
		{
			HealthAndDamage HDScript = blue.GetComponent<HealthAndDamage>();
			HDScript.myHealth = HDScript.previousHealth;
		}

		// Disable this script as we only needed it once
		enabled = false;
	}
}
