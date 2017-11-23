using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonInterface : MonoBehaviour
{
    public GameObject app_menu;

    // Opening settings
    public int openingTimer;
    //  public Collider OpeningStartCollideBox;
    //  public Collider OpeningEndCollideBox;

    // Closing setting
    public int closingTimer;

    // public Collider CloseingStartCollideBox;
    // public Collider CloseingEndCollideBox;

    public int remainingTime;

    // Detection Machine State
    public enum eStates { MenuClosed, HandReadyToOpen, MenuOpened, HandReadyToClose };
    public eStates currentState;

    public bool startOpening, endOpening, startClosing, endClosing;

	int openTry = 0;
	int closeTry = 0;

    // Use this for initialization
    void Start()
    {
        currentState = eStates.MenuClosed;
        app_menu.SetActive(false);
        startOpening = endOpening = startClosing = endClosing = false;
    }

	void FixedUpdate()
    {
		--remainingTime;

		print ("Etat automate : "+ currentState);
		print ("Etat menu : "+ app_menu.activeInHierarchy);

        switch (currentState)
        {
			case eStates.MenuClosed:
				if (app_menu.activeInHierarchy == true) {
					app_menu.SetActive (false);
				}

                if(startOpening)
                {
                    currentState = eStates.HandReadyToOpen;
                    remainingTime = openingTimer;
					++openTry;
					print ("Tentative d'ouverture n°"+openTry);
                }
                break;
			case eStates.HandReadyToOpen:
				if (endOpening) {
					currentState = eStates.MenuOpened;
					print ("Tentative d'ouverture n°" + openTry + " réussie");
					break;
				}

				if (remainingTime <= 0) {
					currentState = eStates.MenuClosed;
					print ("Tentative d'ouverture n°" + openTry + " avortée");

				}
                break;
			case eStates.MenuOpened:
				if (app_menu.activeInHierarchy == false) {
					app_menu.SetActive (true);
					app_menu.GetComponent<MenuPosition> ().Teleport ();

				}

                if(startClosing)
                {
                    currentState = eStates.HandReadyToClose;
                    remainingTime = closingTimer;
					++closeTry;
					print ("Tentative de fermeture n°"+closeTry+"");
				}
                break;
			case eStates.HandReadyToClose:
				if (endClosing) {
					currentState = eStates.MenuClosed;

					print ("Tentative de fermeture n°" + closeTry + " réussie");
					break;

				}

				if (remainingTime <= 0) {
					currentState = eStates.MenuOpened;
					print ("Tentative de fermeture n°" + closeTry + " avortée");
						
				}

                break;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "OpenMenuStart")
            startOpening = true;
        if (other.name == "OpenMenuEnd")
            endOpening = true;
        if (other.name == "CloseMenuStart")
            startClosing = true;
        if (other.name == "CloseMenuEnd")
            endClosing = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "OpenMenuStart")
            startOpening = false;
        if (other.name == "OpenMenuEnd")
            endOpening = false;
        if (other.name == "CloseMenuStart")
            startClosing = false;
        if (other.name == "CloseMenuEnd")
            startClosing = false;
    }
}
