using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    [Serializable]
    public class MenuEntry { public string Name; public GameObject Section; }
    public List<MenuEntry> MenuContent;
    public GameObject SubmenuButtonTemplate;

    [Tooltip("Sert a désactiver les interactions pour eviter de faire nawak pendant une tentative de fermeture")]
    public bool InteractionsEnabled = true;

	// Use this for initialization
	void Start () {
        InitializeSubmenuButtons();
	}

    private void InitializeSubmenuButtons()
    {
        Transform submenusButtonContainer = transform.Find("SubmenuButtons");

        var children = new List<GameObject>();
        foreach (Transform child in submenusButtonContainer) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        foreach(var submenu in MenuContent)
        {

        }
    }

    // Update is called once per frame
    void Update () {

    }
}
