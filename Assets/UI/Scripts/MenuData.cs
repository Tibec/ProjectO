using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class MenuData : MonoBehaviour {

    public bool Enabled;

    private Animation anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animation>();
        anim.playAutomatically = false;
        anim.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
