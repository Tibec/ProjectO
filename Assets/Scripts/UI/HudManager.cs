using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudManager : MonoBehaviour {

    public float MenuInclinaison;

    public float PopupRadius;
    public float MenuRadius;

    public List<GameObject> OpenedMenu = new List<GameObject>();

	// Use this for initialization
	void Start () {
        OpenedMenu.Clear();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddMenu(GameObject content)
    {
        // 1. rotate the active menu

        // 2. instantiate content, Play animation OpenMenu once
    }

    public bool PopupConfirm(string content, bool warning)
    {
        return true;
    }

    public void PopupInfo(string content)
    {

    }
}
