using UnityEngine;
using System.Collections;

/// <summary>
/// This script is attached to the Trigger GameObject on the player and
/// it manages the health of the player accross the network and applies
/// damage to the player accross the network.
/// 
/// This script accesses the PlayerDatabase script to check the player
/// list.
/// 
/// This script is accessed by the BlasterScript
/// 
/// This script accesses the SpawnScript to inform the player that they
/// have been destroyed
/// 
/// This script is accessed by the StatDisplay script 
/// 
/// This script is accessed by the PlayerLabel script
/// 
/// All code is taken from Gamer To Game Developer Series 1
/// http://www.gamertogamedeveloper.com
/// No credit is taken myself
/// </summary>

public class HealthAndDamage : MonoBehaviour {

	// Variables start________________________
	private GameObject parentObject;

	// Used to figure out on who's compute the damage should be applied
	public string myAttacker;
	public bool iWasJustAttacked;

	// These variables are used in figuring out what the player has been
	// hit by and how much damage to apply
	public bool hitByBlaster = false;
	private float blasterDamage = 30;

	// This is used to prevent the player from getting hit while they are 
	// undergroing destruction
	private bool destroyed = false;

	// These variables are used in managing the player's health
	public float myHealth = 100;
	public float maxHealth = 100;
	private float healthRegenRate = 1.3f;
	public float previousHealth = 100;

	// Variables end__________________________

	// Use this for initialization
	void Start () 
	{
		// The trigger GameObject is used in hit detection, but it is
		// the parent GameObject that needs to be destroyed, if the player's
		// health fall below zero
		parentObject = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// If the player is hit by an opposing team projectile, then that projectile
		// will have set iWasJustAttacked to true
		if(iWasJustAttacked == true)
		{
			GameObject gameManager = GameObject.Find("GameManager");
			PlayerDatabase dataScript = gameManager.GetComponent<PlayerDatabase>();

			// Sift through the player list, and only carry out hit detection if
			// the attacking player is the one running this game instance
			for(int i = 0; i < dataScript.PlayerList.Count; i++)
			{
				if(myAttacker == dataScript.PlayerList[i].playerName)
				{
					if(int.Parse(Network.player.ToString()) == dataScript.PlayerList[i].networkPlayer)
					{
						// Check what the player was hit by and apply the damage
						if(hitByBlaster == true && destroyed == false)
						{
							myHealth = myHealth - blasterDamage;
							// Send out an RPC so that this player's attacker is updated
							// accross the network. This way, the attack can recieve a score
							// for destroying the enemy player
							networkView.RPC("UpdateMyCurrentAttackerEverywhere", RPCMode.Others,
							                 myAttacker);

							// Send out an RPC so that this player's health is reduced
							// across the network
							networkView.RPC("UpdateMyCurrentHealthEverywhere", RPCMode.Others,
							                myHealth);
						}
					}
				}
			}
			iWasJustAttacked = false;
		}

		// Each player is responsible for destroying themselves
		if(myHealth <=0 && networkView.isMine == true)
		{
			// Access the spawn script and set the iAmDestroyed bool
			// to true so that this player can respawn
			GameObject spawnManager = GameObject.Find("SpawnManager");
			SpawnScript spawnScript = spawnManager.GetComponent<SpawnScript>();
			spawnScript.iAmDestroyed = true;

			// Remove this player's RPCs. If we didn't do this
			// a ghost of the player would remain in the game
			// which would be very confusing for players just
			// connecting
			Network.RemoveRPCs(Network.player);

			// Send an RPC to destroy our player accross the 
			// network
			networkView.RPC("DestroySelf", RPCMode.All);
		}

		// If the player's health is different from their previous health then
		// update the health record across the network and buffer it
		if(myHealth > 0 && networkView.isMine == true)
		{
			if(myHealth != previousHealth)
			{
				networkView.RPC("UpdateMyHealthRecordEverywhere", RPCMode.AllBuffered, myHealth);
			}
		}

		// Regen the player's health if it is below the max health
		if(myHealth < maxHealth)
		{
			myHealth = myHealth + healthRegenRate*Time.deltaTime;
		}
		
		// If the player's health exceeds the max health while regenerating
		// then set it back to the max health
		if(myHealth > maxHealth)
		{
			myHealth = maxHealth;
		}
	}

	[RPC]
	void UpdateMyCurrentAttackerEverywhere(string attacker)
	{
		myAttacker = attacker;
	}

	[RPC]
	void UpdateMyCurrentHealthEverywhere(float health)
	{
		myHealth = health;
	}

	[RPC]
	void DestroySelf()
	{
		Destroy(parentObject);
	}

	[RPC]
	void UpdateMyHealthRecordEverywhere(float health)
	{
		previousHealth = health;
	}
}
