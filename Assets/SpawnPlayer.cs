using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject.Find("Player").transform.position = transform.position;
        GameObject.Find("Bulle").transform.position = transform.position;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
