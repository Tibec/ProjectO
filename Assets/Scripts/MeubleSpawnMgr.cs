using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeubleSpawnMgr : MonoBehaviour {

    public GameObject meubleContainer;
    public Transform playerHead;
    public float spawnDistance;
    public GameObject meubleMenuPrefab;

    // Use this for initialization
    void Start () {
        //DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Spawn(MeubleMetadata metadata)
    {
        float r = playerHead.transform.rotation.eulerAngles.y * Mathf.PI / 180.0f;
        Vector3 frontPlayer = new Vector3();

        frontPlayer.x = playerHead.transform.position.x + (spawnDistance * Mathf.Sin(r));
        frontPlayer.y = playerHead.transform.position.y + 1;
        frontPlayer.z = playerHead.transform.position.z + (spawnDistance * Mathf.Cos(r));

        // Instantiating
        GameObject newGo = Instantiate(meubleContainer, frontPlayer, Quaternion.Euler(0,0,0));
        GameObject newMeuble = Instantiate(metadata.meubleObject, newGo.transform);
        newGo.transform.localScale = Vector3.one * metadata.initialScale;

        // Adjusting meuble properties
        MeubleInteraction mi = newGo.GetComponent<MeubleInteraction>();
        mi.minScale = metadata.minScale;
        mi.maxScale = metadata.maxScale;
        mi.meubleMenuPrefab = meubleMenuPrefab;

        // Setting as child
        newGo.transform.parent = transform;
    }
}
