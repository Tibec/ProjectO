using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMenu : MonoBehaviour {

    public GameObject menu;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnGestureDetected()
    {
        FindObjectOfType<HudManager>().AddMenu(menu);
    }
}
