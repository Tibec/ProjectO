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
				//the hand is around the chest, but the chest doesn't virtually exist. We use the z-coordinate of the camera.
			if (IsHandAroundTheChest(closeMargin))
			{
				currentState = eStates.HandReadyToClose;
			}
                break;
		case eStates.HandReadyToClose:
			if (handMovement.speed.z > requiredCloseSpeed) 
			{
				if (handMovement.speed.x < openMargin && handMovement.speed.y < openMargin)
				{
					print("ON A FERME LE MENU OMG!");
					currentState = eStates.MenuClosed;
				}
			
			}
				
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
	private bool IsHandAroundTheChest (float margin) //la même que head, mais pour préparer la fermeture du menu
	{
		return transform.position.z > playerHead.transform.position.z - openMargin &&
			transform.position.z < playerHead.transform.position.z + openMargin;
	
	}
}
