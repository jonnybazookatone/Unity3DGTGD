using UnityEngine;
using System.Collections;

/// <summary>
/// This script is attached to the player and allows them
/// to fire the projectile
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

	// Variable end_______________________

	// Use this for initialization
	void Start () {

		myTransform = transform;
		cameraHeadTransform = myTransform.FindChild("CameraHead");
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetButton("FireWeapon") && Time.time > nextFire)
		{
			nextFire = Time.time + fireRate;
			// The launch position of the projectile will just be
			// infront of the CameraHead
			launchPosition = cameraHeadTransform.TransformPoint(0, 0, 0.2f);

			// Create the blaster projectile at the launchPosition
			// and tilt its angle so that it's horizontal using
			// the angle 
			Instantiate(Blaster, launchPosition, Quaternion.Euler(cameraHeadTransform.eulerAngles.x + 90,
			                                                      myTransform.eulerAngles.y, 0));
		}
	}
}
