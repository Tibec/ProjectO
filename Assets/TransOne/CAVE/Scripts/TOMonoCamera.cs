using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Simple script to signal that this camera is to be used as a TONode
///It is disabled until it is required
/// </summary>
public class TOMonoCamera : MonoBehaviour {

	public int idNode;

	void Start () {
		GetComponent<Camera> ().enabled = false;
	}
		
	void Update () {
		
	}
}
