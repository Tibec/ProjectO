using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GestureDetectionStep : MonoBehaviour {
    public int id;
    public GestureDetectionAttempt attempt;
    public float delayToReachNextStep;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

    }

    private void OnTriggerEnter(Collider other)
    {
        attempt.SendMessage("OnStepTriggered", id);
    }
}
