using UnityEngine;
using System.Collections;

/// <summary>
/// This script is attached to the player and it draws the
/// health bar of the player above them
/// 
/// This script acceses the HealthAndDamage script for
/// determining the health bar length
/// 
/// This script is accessed by the PlayerName script
/// 
/// All code is taken from Gamer To Game Developer Series 1
/// http://www.gamertogamedeveloper.com
/// No credit is taken myself
/// </summary>

public class PlayerLabel : MonoBehaviour {

	// Variables start____________________

	// The health bar texture is attached to this in the inspector
	public Texture healthTex;

	// References to camera and player position
	private Camera myCamera;
	private Transform myTransform;
	private Transform triggerTransform;
	private HealthAndDamage HDScript;

	// These are used in determining whether the health bar should be
	// drawn and where on the screen
	private Vector3 worldPosition = new Vector3();
	private Vector3 screenPosition = new Vector3();
	private Vector3 cameraRelativePosition = new Vector3();
	private float minimumZ = 1.5f;

	// These variables are used in defining the health bar
	private float labelTop = 10;
	private float labelWidth = 110;
	private int labelHeight = 15;
	private int barTop = 1;
	private int healthBarHeight = 5;
	private int healthBarLeft = 110;
	private float healthBarLength;
	private float adjustment = 1;
	private float boxLeft;
	private float boxRight;
	private float labelBoxLeft;
	private float labelBoxTop;

	// Used in displaying the player's name
	public string playerName;
	private GUIStyle myStyle = new GUIStyle();

	// Variables end______________________

	void Awake ()
	{
		// This script will only run for the other players.
		// We do not need a health bar being drawn above our own player
		// in our game
		if(networkView.isMine == false)
		{
			myTransform = transform;
			myCamera = Camera.main;
			
			// Access the HealthAndDamage script
			Transform triggerTransform = transform.FindChild("Trigger");
			HDScript = triggerTransform.GetComponent<HealthAndDamage>();
			
			// The font colour of the GUIStyle depends on which team
			// the player is on
			if(myTransform.tag == "BlueTeam")
			{
				myStyle.normal.textColor = Color.blue;
			}
			
			if(myTransform.tag == "RedTeam")
			{
				myStyle.normal.textColor = Color.red;
			}
			
			myStyle.fontSize = 12;
			myStyle.fontStyle = FontStyle.Bold;
			
			// Allow the text to extend beyond the width of the label
			myStyle.clipping = TextClipping.Overflow;
		}
		else
		{
			enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Find out whether the player is infront or behind the camera
		cameraRelativePosition = myCamera.transform.InverseTransformPoint(myTransform.position);

		// Figure out how long the health bar should be and to avoid a drawing error
		// the health bar length is set to 1 if the health falls below 1
		if(HDScript.myHealth < 1)
		{
			healthBarLeft = 1;
		}

		if(HDScript.myHealth >= 1)
		{
			healthBarLength = (HDScript.myHealth / HDScript.maxHealth) * 100;
		}

	}

	void OnGUI ()
	{
		// Only display the player's name if they are infront of the camera and also
		// the player should be infront of the camera by at least minimumZ
		if(cameraRelativePosition.z > minimumZ)
		{
			// Set the world position to be just above the player
			worldPosition = new Vector3(myTransform.position.x, myTransform.position.y+adjustment,
			                            myTransform.position.z);

			// Convert the world position to a point on the screen
			screenPosition = myCamera.WorldToScreenPoint(worldPosition);

			// Draw the health bar and the grey bar behind it
			boxLeft = screenPosition.x - healthBarLeft / 2;
			boxRight = Screen.height - screenPosition.y - barTop;
			GUI.Box (new Rect(boxLeft, boxRight, 100, healthBarHeight), "");
			GUI.DrawTexture(new Rect(boxLeft, boxRight, healthBarLength, healthBarHeight), healthTex);

			// Draw the player's name above them
			labelBoxLeft = screenPosition.x - labelWidth / 2;
			labelBoxTop = Screen.height - screenPosition.y + labelTop;
			GUI.Label(new Rect(labelBoxLeft, labelBoxTop, labelWidth, labelHeight), 
			          playerName, myStyle);
		}
	}
}
