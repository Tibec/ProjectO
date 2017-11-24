using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeubleMenuPosition : MonoBehaviour {

    public Vector3 attachPoint;
    public Transform attachedObject;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position =  attachedObject.position + attachPoint;
	}
}
