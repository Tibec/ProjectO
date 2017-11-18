using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MeubleEntry
{
    public string displayName;
    public GameObject Object;
    public int initialScale;
    public Sprite preview;
}

public class MeubleMgr : MonoBehaviour {

    public List<MeubleEntry> meubles;
    public GameObject itemTemplate;

    public List<Vector3> itemSlot;
    
    // Use this for initialization
    void Start () {
        LoadMeubles(0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LoadMeubles(int offset)
    {
        // DeleteOldItems
        var children = new List<GameObject>();
        foreach (Transform child in transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        // AddNewItem
        if (meubles != null && itemSlot != null)
        {
            for (int i = 0; i < itemSlot.Count && i+offset < meubles.Count; ++i)
            {
                GameObject go = Instantiate(itemTemplate, transform);
                Meuble m = go.GetComponent<Meuble>();
                m.SetData(meubles[offset+i]);
                go.transform.localPosition = itemSlot[i];
            }
        }
    }
}
