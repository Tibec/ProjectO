using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour {

    MenuData mgr;
    public GameObject eventHandler;

	// Use this for initialization
	void Start () {
        mgr = GetComponentInParent<MenuData>();
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
            if (mgr.Enabled && eventHandler != null)
                eventHandler.SendMessage("OnClick", this);
            if (mgr.Enabled && eventHandler == null)
                SendMessage("OnClick", this);
        }
    }
}
