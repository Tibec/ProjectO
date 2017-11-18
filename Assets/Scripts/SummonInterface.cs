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

    private int remainingTime;

    // Detection Machine State
    public enum eStates { MenuClosed, HandReadyToOpen, MenuOpened, HandReadyToClose };
    public eStates currentState;

    private bool startOpening, endOpening, startClosing, endClosing;


    // Use this for initialization
    void Start()
    {
        currentState = eStates.MenuClosed;
        app_menu.SetActive(false);
        startOpening = endOpening = startClosing = endClosing = false;
    }

    // Update is called once per frame
    void Update()
    {

        switch (currentState)
        {
            case eStates.MenuClosed:
                if(startOpening)
                {
                    currentState = eStates.HandReadyToOpen;
                    remainingTime = openingTimer;
                }
                break;
            case eStates.HandReadyToOpen:
                if(endOpening)
                {
                    currentState = eStates.MenuOpened;
                    app_menu.SetActive(true);
                }

                --remainingTime;
                if (remainingTime <= 0)
                    currentState = eStates.MenuClosed;
                break;
            case eStates.MenuOpened:
                if(startClosing)
                {
                    currentState = eStates.HandReadyToClose;
                    remainingTime = closingTimer;
                }
                break;
            case eStates.HandReadyToClose:
                if (endClosing)
                {
                    currentState = eStates.MenuClosed;
                    app_menu.SetActive(false);
                }

                --remainingTime;
                if (remainingTime <= 0)
                    currentState = eStates.MenuOpened;
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
