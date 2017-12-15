using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnClick(MenuButton btn)
    {
        if(btn.name == "Left")
        {
            GetComponentInParent<HudManager>().Rotate(true);
        }
        else if(btn.name == "Right")
        {
            GetComponentInParent<HudManager>().Rotate(false);
        }
    }
}
