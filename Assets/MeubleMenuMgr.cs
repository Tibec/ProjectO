﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeubleMenuMgr : MonoBehaviour {

    public Transform attachPoint;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = attachPoint.position;
	}
}
