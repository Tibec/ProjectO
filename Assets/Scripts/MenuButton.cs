using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour {

    MenuManager mgr;
    public GameObject eventHandler;

	// Use this for initialization
	void Start () {
        mgr = FindObjectOfType<MenuManager>();
        if (mgr == null)
            Debug.LogError("Impossible de trouver le gestionnaire de menu");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (mgr.InteractionsEnabled && eventHandler != null)
                eventHandler.SendMessage("OnClick", this);
            if (mgr.InteractionsEnabled && eventHandler == null)
                SendMessage("OnClick", this);
        }
    }
}
