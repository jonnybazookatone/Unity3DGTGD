using UnityEngine;
using System.Collections;

/// <summary>
/// This script is attached to the player
/// and allows the player to see a box with their
/// current health to the lower right of the crosshair
/// 
/// This script accesses the HealthAndDamage script
/// 
/// All code is taken from Gamer To Game Developer Series 1
/// http://www.gamertogamedeveloper.com
/// No credit is taken myself
/// </summary>

public class StatDisplay : MonoBehaviour {

	// Variables start____________________

	// The healthbar texture is attached to this 
	// in the inspector
	public Texture healthTex;

	// These are used in calculating and displaying
	// the health
	private float health;
	private int healthForDisplay;

	// These are used in defining the StatDisplay box
	private int boxWidth = 160;
	private int boxHeight = 85;
	private int labelHeight = 20;
	private int labelWidth = 35;
	private int padding = 10;
	private int gap = 120;
	private float healthBarLength;
	private int healthBarHeight = 15;
	private GUIStyle healthStyle = new GUIStyle();
	private float commonLeft;
	private float commonTop;

	// A quick reference to the HealthAndDamage script
	private HealthAndDamage HDScript;

	// Variables end______________________


	// Use this for initialization
	void Start () 
	{
		if(networkView.isMine == true)
		{
			// Access the HealthAndDamage script
			Transform triggerTransform = transform.FindChild("Trigger");
			HDScript = triggerTransform.GetComponent<HealthAndDamage>();

			// Set the GUI style
			healthStyle.normal.textColor = Color.green;
			healthStyle.fontStyle = FontStyle.Bold;
		}
		else
		{
			enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Access the HealthAndDamage script continuously and retrieve
		// the player's current health
		health = HDScript.myHealth;

		// I also want to display health as a number, without any decimals
		healthForDisplay = Mathf.CeilToInt(health);

		// Calculate how long the health bar should be. The max length is 100
		// representing 100 percent
		healthBarLength = (health / HDScript.maxHealth) * 100;
	}

	void OnGUI ()
	{
		commonLeft = (Screen.width / 2) + 180;
		commonTop = (Screen.height / 2) + 50;

		// Draw a plane box behind the health bar
		GUI.Box(new Rect(commonLeft, commonTop, boxWidth, boxHeight), "");

		// Draw a grey box behind the health bar 
		GUI.Box(new Rect(commonLeft+padding, commonTop+padding, 100, healthBarHeight), ""); 

	}
}
