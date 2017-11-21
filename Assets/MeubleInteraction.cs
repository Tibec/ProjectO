using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeubleInteraction : MonoBehaviour {

    public float minScale;
    public float maxScale;

    public GameObject meubleMenuPrefab;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnRemoteCollisionEnter(CollisionListenerData data)
    {
        Instantiate(meubleMenuPrefab, transform.Find("MenuSpawn"));
    }

    void OnRemoteCollisionExit(CollisionListenerData data)
    {

    }
}
