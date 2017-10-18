using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectHandMovement : MonoBehaviour {

	public Vector3 acceleration;
	public int CachedPosition;

	public Queue<Vector3> lastPositions = new Queue<Vector3> ();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		// Updates positions queue
		lastPositions.Enqueue (transform.position);

		if (lastPositions.Count > CachedPosition)
			lastPositions.Dequeue ();

		// update acceleration
		Vector3 initialPosition = lastPositions.Peek();
		acceleration = Vector3.zero;
		foreach (Vector3 position in lastPositions) {
			acceleration += position - initialPosition;
		}
	}
}
