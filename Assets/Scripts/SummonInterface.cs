using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonInterface : MonoBehaviour {

    public Camera playerHead;
    public Transform menu;

	public float requiredOpenSpeed = 0.1f;
	public float openMargin = 0.1f;
	public float requiredCloseSpeed;
	public float closeMargin;

    ComputeRigidBodyMovement handMovement;

    // Detection Machine State
    public enum eStates { MenuClosed, HandReadyToOpen, MenuOpened, HandReadyToClose };
    public eStates currentState;



	// Use this for initialization
	void Start () {
		handMovement = GetComponent<ComputeRigidBodyMovement> ();
	}
	
	// Update is called once per frame
	void Update () {

        switch(currentState)
        {
            case eStates.MenuClosed:
                // the hand is around the head
                if (IsHandAroundTheHead(openMargin))
                {
                    currentState = eStates.HandReadyToOpen;
                }
                break;
            case eStates.HandReadyToOpen:
                if (handMovement.speed.y >  requiredOpenSpeed)
                {
                    if (handMovement.speed.x < openMargin && handMovement.speed.z < openMargin)
                    {
                        print("ON A OUVERT LE MENU OMG!");
                        currentState = eStates.MenuOpened;
                    }
                }
                break;
            case eStates.MenuOpened:
                break;
            case eStates.HandReadyToClose:
                break;
        }
/*
        // Detection ouverture
        if (!menuSummoned)
		{
			Vector3 lastSpeed = handMovement.speed;

		}
		else // Detection fermeture
		{
			Vector3 lastAccel = handMovement.speed;
			if (lastAccel.x > requiredCloseSpeed) 
			{
				if (lastAccel.y < closeMargin && lastAccel.z < closeMargin)
				{
					print ("ON A FERMER LE MENU OMG!");
				}
			}

		}*/
	}

    private bool IsHandAroundTheHead(float margin)
    {
        return transform.position.y > playerHead.transform.position.y - openMargin &&
                    transform.position.y < playerHead.transform.position.y + openMargin;
    }
}
