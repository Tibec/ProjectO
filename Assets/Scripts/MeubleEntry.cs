using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class MeubleEntry : MonoBehaviour {

    public MeubleMetadata metadata;


    // Use this for initialization
    void Start () {

    }
	
    public void SetData(MeubleMetadata _metadata)
    {
        metadata = _metadata;

        Image previewContainer = transform.Find("Preview").GetComponent<Image>();
        previewContainer.sprite = _metadata.preview;
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnClick()
    {
        MeubleSpawnMgr g = FindObjectOfType<MeubleSpawnMgr>();

        g.Spawn(metadata);
    }
}
