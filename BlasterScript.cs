using UnityEngine;
using System.Collections;

/// <summary>
/// This script is attached to the Blaster projectile 
/// and it governs the behaviour of the projectile.
/// 
/// All code is taken from Gamer To Game Developer Series 1
/// http://www.gamertogamedeveloper.com
/// No credit is taken myself
/// 
/// </summary>

public class BlasterScript : MonoBehaviour {

	// Variables start______________________
	
	// The explosion effect is attached to this object in the inspector
	public GameObject BlasterExplosion;

	// Quick reference to projectile
	private Transform myTransform;

	// The projectiles flight speed
	private float projectileSpeed = 10;

	// Prevent the projectile from causing 
	// further harm once it has his something
	private bool expended = false;

	// A ray projected in front of the projectile
	// to see if it will hit a recognisable collider
	private RaycastHit hit;

	// The range of the ray
	private float range = 1.5f;

	// The life span of the projectile
	private float expireTime = 5;
	
	// Variables end________________________

	// Use this for initialization
	void Start () 
	{
		// Set the transform to the transform that this script belongs to
		myTransform = transform;

		// As soon as the projectile is created, start a countdown to 
		// destroy it
		StartCoroutine(DestroyMyselfAfterSomeTime());
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Translate the projectile in the UP direction (the pointed
		// end of the projectile).
		// v = d/t; new_d = v * time
		myTransform.Translate(Vector3.up * projectileSpeed * Time.deltaTime);

		// If the ray hits something then execute this code
		if(Physics.Raycast(myTransform.position, myTransform.up, out hit, range) && 
		   expended == false)
		{
			// If the collider has the tag of Floor then..
			if(hit.transform.tag == "Floor")
			{
				// Instantiate an explosion effect
				Instantiate(BlasterExplosion, hit.point, Quaternion.identity);

				expended = true;

				// Make the projectile become invisible
				myTransform.renderer.enabled = false;

				// Turn off its light so that the halo also dissapears
				myTransform.light.enabled = false;
			}
		}
	}

	IEnumerator DestroyMyselfAfterSomeTime()
	{
		// Wait for the timer to count up to the expireTime
		// and then destroy the projectile

		yield return new WaitForSeconds(expireTime);

		Destroy(myTransform.gameObject);
	}
}
