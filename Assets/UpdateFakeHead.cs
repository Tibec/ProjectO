using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateFakeHead : MonoBehaviour {

	public Transform head;
	public Transform body;

	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (body.position.x, head.position.y, body.position.z);
	}
}
