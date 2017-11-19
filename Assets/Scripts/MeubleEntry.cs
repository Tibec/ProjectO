using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MeubleEntry : MonoBehaviour {

    public MeubleMetadata metadata;


    // Use this for initialization
    void Start () {

    }
	
    public void SetData(MeubleMetadata _metadata)
    {
        metadata = _metadata;

        SpriteRenderer previewContainer = transform.Find("Preview").GetComponent<SpriteRenderer>();
        previewContainer.sprite = _metadata.preview;
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnButtonClick()
    {
        print("On veut ajouter un meuble a l'univers");
    }
}
