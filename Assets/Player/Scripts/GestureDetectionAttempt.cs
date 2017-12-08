using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureDetectionAttempt : MonoBehaviour {

    public GestureDetection manager;
    public float remainingTime;
    public int nextStep = 0;

    // Use this for initialization
    void Start () {

        if (manager.UseGlobalTimer)
        {
            remainingTime = manager.GlobalTimer;
        } 
        else
        {
            remainingTime = manager.GesturePath[nextStep].delayToReachNextStep;
        }

    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        remainingTime -= Time.fixedDeltaTime;

		if(remainingTime <= 0)
        {
            Destroy(gameObject);
        }
	}

    private void OnStepTriggered(int id)
    {
        if(id == nextStep)
        {
            // if last step triggered
            if(id == manager.GesturePath[manager.GesturePath.Count - 1].id)
            {
                manager.SendMessage("OnGestureDetected");
                Destroy(gameObject);
            }
            else
            {
                ++nextStep;
                if(!manager.UseGlobalTimer)
                    remainingTime = manager.GesturePath[nextStep].delayToReachNextStep;
            }
        }
    }

}
