using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMenu : MonoBehaviour {


    public bool open = false;
    public GameObject menu;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnGestureDetected()
    {
        if (FindObjectOfType<MenuManager>() != null)
            return;
        FindObjectOfType<HudManager>().AddMenu(menu);
    }
}
