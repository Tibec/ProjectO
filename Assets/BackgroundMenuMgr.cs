using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMenuMgr : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnClick(MenuButton btn)
    {
        if (btn.name == "Nature")
        {
            FindObjectOfType<BubbleMgr>().ChangeScene("Nature");
        }
        else if (btn.name == "Base")
        {
            FindObjectOfType<BubbleMgr>().ChangeScene("Base");
        }
        else if (btn.name == "Mountain")
        {
            FindObjectOfType<BubbleMgr>().ChangeScene("Mountain");
        }
    }
}
