using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeRigidBodyMovement : MonoBehaviour {
    

    [ReadOnly]
    public Vector3 speed;

    private Vector3 prevPos;

    void Start()
    {
        StartCoroutine(CalcVelocity());
    }

    IEnumerator CalcVelocity()
    {
        while (Application.isPlaying)
        {
            // Position at frame start
            prevPos = transform.position;
            // Wait till it the end of the frame
            yield return new WaitForEndOfFrame();
            // Calculate velocity: Velocity = DeltaPosition / DeltaTime
            speed = (prevPos - transform.position) / Time.deltaTime;
        }
    }
}
