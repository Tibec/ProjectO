using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonInterface : MonoBehaviour {

	DetectHandMovement handMovement;
	bool menuSummoned = false;

	public float requiredOpenSpeed;
	public float openMargin;
	public float requiredCloseSpeed;
	public float closeMargin;

	// Use this for initialization
	void Start () {
		handMovement = GetComponent<DetectHandMovement> ();
	}
	
	// Update is called once per frame
	void Update () {

		// Detection ouverture
		if (!menuSummoned)
		{
			Vector3 lastAccel = handMovement.acceleration;
			if (lastAccel.y > requiredOpenSpeed) 
			{
				if (lastAccel.x < openMargin && lastAccel.z < openMargin)
				{
					print ("ON A OUVERT LE MENU OMG!");
					menuSummoned = true;
				}
			}
		}
		else // Detection fermeture
		{
			Vector3 lastAccel = handMovement.acceleration;
			if (lastAccel.x > requiredCloseSpeed) 
			{
				if (lastAccel.y < closeMargin && lastAccel.z < closeMargin)
				{
					print ("ON A FERMER LE MENU OMG!");
					menuSummoned = false;
				}
			}

		}
	}
}
