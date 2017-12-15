using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    
    [Serializable]
    public class MenubuttonGameobjectPair { public MenuButton Key; public GameObject Value; }
    public List<MenubuttonGameobjectPair> LinkBetweenButtonAndContent = new List<MenubuttonGameobjectPair>();

	// Use this for initialization
	void Start () {
        InitializeSubmenus();

    }

    private void InitializeSubmenus()
    {
        for(int i=0;i<LinkBetweenButtonAndContent.Count;++i)
        {
            if (i == 0)
                LinkBetweenButtonAndContent[i].Value.SetActive(true);
            else
                LinkBetweenButtonAndContent[i].Value.SetActive(false);

        }
    }

    // Update is called once per frame
    void Update () {

    }

    void OnClick(MenuButton btn)
    {
        foreach(var entry in LinkBetweenButtonAndContent)
        {
            if(entry.Key == btn)
                entry.Value.SetActive(true);
            else
                entry.Value.SetActive(false);
        }
    }
}
