using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonInterface : MonoBehaviour {

    public Camera playerHead;
	public GameObject app_menu;
	public float requiredOpenSpeed = 2.0f;
	public float openMargin = 0.2f;
	public float openMarginBound = 0.5f;
	public float requiredCloseSpeed;
	public float closeMargin;

    ComputeRigidBodyMovement handMovement;

    // Detection Machine State
    public enum eStates { MenuClosed, HandReadyToOpen, MenuOpened, HandReadyToClose };
    public eStates currentState;



	// Use this for initialization
	void Start () {
		handMovement = GetComponent<ComputeRigidBodyMovement> ();
		currentState = eStates.MenuClosed;
		app_menu.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		print (handMovement.speed);
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
						app_menu.SetActive (true); // on affiche le menu.
                        currentState = eStates.MenuOpened;
                    }
                }
				if (!IsHandAroundTheHead(openMargin))
				{
					currentState = eStates.MenuClosed;
				}               
			break;
            case eStates.MenuOpened:
				//the hand is around the chest, but the chest doesn't virtually exist. We use the z-coordinate of the camera.
			if (IsHandAroundTheMenu(closeMargin))
			{
				print ("hello");
				currentState = eStates.HandReadyToClose;
			}
                break;
		case eStates.HandReadyToClose:
			if (handMovement.speed.z > requiredCloseSpeed) 
			{
				if (handMovement.speed.x < openMargin && handMovement.speed.y < openMargin)
				{
					print("ON A FERME LE MENU OMG!");
					app_menu.SetActive (false); //on rend le menu invisible.
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
	private bool IsHandAroundTheMenu (float margin) //la même que head, mais pour préparer la fermeture du menu
	{
		return transform.position.z > app_menu.transform.position.z - openMargin &&
			transform.position.z < app_menu.transform.position.z + openMargin;
	
	}
}
