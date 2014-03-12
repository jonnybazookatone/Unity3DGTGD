using UnityEngine;
using System.Collections;

/// <summary>
/// This script is attached to the Player and it causes
/// the camera to continuously follow the camera head.
/// </summary>


public class CameraScript : MonoBehaviour {

	// Variables start___________________________

	private Camera myCamera;
	private Transform cameraHeadTransform;

	// Variables end_____________________________


	// Use this for initialization
	void Start () 
	{
		if(networkView.isMine == true)
		{
			myCamera = Camera.main;
			cameraHeadTransform = transform.FindChild("CameraHead");
		}
		else
		{
			enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Make the camera follow the Player's camera head transform
		myCamera.transform.position = cameraHeadTransform.position;
		myCamera.transform.rotation = cameraHeadTransform.rotation;
	}
}
