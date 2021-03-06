﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookPlayer : MonoBehaviour {

    public Transform playerHead;

	// Use this for initialization
	void Start () {
        if(playerHead == null)
            playerHead = FindObjectOfType<TOCAVEController>().transform;

    }
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(playerHead, Vector3.up);
        transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x * -1 ,
            transform.rotation.eulerAngles.y + 180,
            transform.rotation.eulerAngles.z);

    }
}
