using UnityEngine;
using System.Collections;

/// <summary>
/// This script is attached to the player and allows them
/// to fire the projectile
/// 
/// This script accesses the SpawnSCript
/// 
/// This script accesses the BlasterScript of a newly instantiated blaster projectile
/// 
/// All code is taken from Gamer To Game Developer Series 1
/// http://www.gamertogamedeveloper.com
/// No credit is taken myself
/// 
/// </summary>


public class FireBlaster : MonoBehaviour {

	// Variable start_____________________

	// The blaster projectile is attached to this in the inspector
	public GameObject Blaster;

	// Quick references
	private Transform myTransform;
	private Transform cameraHeadTransform;

	// The position at which the projectile should be instantiated
	private Vector3 launchPosition = new Vector3();

	// Used to control the rate of fire
	private float fireRate = 0.2f;
	private float nextFire = 0;

	// Used to determine which team the player is on
	private bool iAmOnTheBlueTeam = false;
	private bool iAmOnTheRedTeam = false;

	// Variable end_______________________

	// Use this for initialization
	void Start () {
		if(networkView.isMine == true)
		{
			myTransform = transform;
			cameraHeadTransform = myTransform.FindChild("CameraHead");
		}
		else
		{
			enabled = false;
		}

		// Find the SpawnManager and access the SpawnScript to 
		// find out which team the player is on
		GameObject spawnManager = GameObject.Find("SpawnManager");
		SpawnScript spawnScript = spawnManager.GetComponent<SpawnScript>();

		if(spawnScript.amIOnTheRedTeam == true)
		{
			iAmOnTheRedTeam = true;
		}

		if(spawnScript.amIOnTheBlueTeam == true)
		{
			iAmOnTheBlueTeam = true;
		}
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetButton("FireWeapon") && Time.time > nextFire && Screen.lockCursor == true)
		{
			nextFire = Time.time + fireRate;
			// The launch position of the projectile will just be
			// infront of the CameraHead
			launchPosition = cameraHeadTransform.TransformPoint(0, 0, 0.2f);

			// Create the blaster projectile accross the network at the launchPosition
			// and tilt its angle so that it's horizontal using
			// the angle
			// Also, make it team specific
			if(iAmOnTheRedTeam == true)
			{
				networkView.RPC("SpawnProjectile", RPCMode.All, launchPosition, 
				                Quaternion.Euler(cameraHeadTransform.eulerAngles.x + 90, 
				                 myTransform.eulerAngles.y, 0), 
				                myTransform.name, "red");
			}

			if(iAmOnTheBlueTeam == true)
			{
				networkView.RPC("SpawnProjectile", RPCMode.All, launchPosition, 
				                Quaternion.Euler(cameraHeadTransform.eulerAngles.x + 90, 
				                 myTransform.eulerAngles.y, 0), 
				                myTransform.name, "blue");
			}
		}

	}

	[RPC]
	void SpawnProjectile(Vector3 position, Quaternion rotation, 
	                     string originatorName, string team)
	{
		// Access the BlasterScript of the newly instantiated
		// Blaster projectile and supply the player's name
		// and team
		GameObject go = Instantiate(Blaster, position, rotation) as GameObject;
		BlasterScript bScript = go.GetComponent<BlasterScript>();
		bScript.myOriginator = originatorName;
		bScript.team = team;
	}
}

