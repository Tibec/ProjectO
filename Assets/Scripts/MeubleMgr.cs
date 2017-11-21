using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MeubleMetadata
{
    public string displayName;
    public GameObject meubleObject;
    public float initialScale;
    public float minScale;
    public float maxScale;
    public Vector3 menuPosition;
    public Sprite preview;
}

public class MeubleMgr : MonoBehaviour {

    public List<MeubleMetadata> meubles;
    public GameObject itemTemplate;

    public List<Vector3> itemSlot;

    private int currentPage;
    private int maxPage;

    // Use this for initialization
    void Start () {
        maxPage = (meubles.Count - 1 ) / itemSlot.Count ;
        LoadMeublePage(0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LoadMeublePage(int i)
    {
        if (i > maxPage)
            i = 0;
        else if (i < 0)
            i = maxPage;

        LoadMeubles(i * itemSlot.Count);
        currentPage = i;
    }

    private void LoadMeubles(int offset)
    {
        // DeleteOldItems
        var children = new List<GameObject>();
        foreach (Transform child in transform.Find("Items")) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        // AddNewItem
        if (meubles != null && itemSlot != null)
        {
            for (int i = 0; i < itemSlot.Count && i+offset < meubles.Count; ++i)
            {
                GameObject go = Instantiate(itemTemplate, transform.Find("Items"));
                go.name = (i + offset).ToString();
                MeubleEntry m = go.GetComponent<MeubleEntry>();
                m.SetData(meubles[offset+i]);
                go.transform.localPosition = itemSlot[i];
            }
        }
    }

    void OnClick(MenuButton btn)
    {
        if (btn.name == "NextBtn")
        {
            LoadMeublePage(currentPage + 1);
        }
        else if (btn.name == "PrevBtn")
        {
            LoadMeublePage(currentPage - 1);
        }
        else
        {
            int.Parse(btn.name);
        }
    }
}
