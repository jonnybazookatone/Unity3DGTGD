using UnityEngine;
using System.Collections;

/// <summary>
/// This script is attached to the player
/// and causes a crosshair to be drawn
/// to the screen
/// 
/// All code is taken from Gamer To Game Developer Series 1
/// http://www.gamertogamedeveloper.com
/// No credit is taken myself
/// </summary>

public class Crosshair : MonoBehaviour {

	// Variables start__________________
	public Texture crosshairTex;
	private float crosshairDimension = 256;
	public float rectHeight;
	public float rectWidth;

	// Variables end____________________

	// Use this for initialization
	void Start () 
	{
		if(networkView.isMine == false)
		{
			enabled = false;
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI ()
	{
		// Display the crosshair in the center of the screen
		// while the cursor is locked
		if(Screen.lockCursor == true)
		{
			rectHeight = (Screen.height - crosshairDimension) / 2;
			rectWidth = (Screen.width - crosshairDimension) / 2;
			GUI.DrawTexture(new Rect(rectWidth, rectHeight, crosshairDimension, crosshairDimension),
			                crosshairTex);
		}
	}
}
