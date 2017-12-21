using System;
using System.Collections;
using System.Collections.Generic;


using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class GestureStep
{
    public Collider collider;
    [Tooltip("Temps en secondes, ignoré si UseGlobalTimer = true")]
    public float maxDelayToReachNextCollider;
}

[RequireComponent(typeof(Collider))]
public class GestureDetection : MonoBehaviour {

    public string GestureName;

    public bool UseGlobalTimer;
    public float GlobalTimer;
    private Collider StartArea;
    public Transform StepContainer;
    [SerializeField]
    public List<GestureDetectionStep> GesturePath;

    // private int lastTriggerId = -1;
    // private float remainingTime = 0;

	// Use this for initialization
	void Start ()
    {
        // lastTriggerId = -1;

        // We don't need it, it be copied to the right position for each attempt;
        StepContainer.gameObject.SetActive(false);

        Collider[] coll = GetComponents<Collider>();
        bool containAtLeastOneTrigger = false;
        foreach (var c in coll)
            if (c.isTrigger)
                containAtLeastOneTrigger = true;
        Assert.IsTrue(containAtLeastOneTrigger, "There is no trigger assigned to the gesture detection called : " + GestureName);
    }


    void FixedUpdate ()
    {

    }

    void OnGestureDetected()
    {
        print("bave");
    }

    void OnRemoteCollisionEnter(CollisionListenerData data)
    {/*
        if (lastTriggerId + 1 > GesturePath.Count)
            return;

        if (data.sender == GesturePath[lastTriggerId + 1].gameObject)
        {
            if(data.collision.tag == "Player")
            {
                ++lastTriggerId;

                if (lastTriggerId == 0)
                    remainingTime = GlobalTimer;

                if (lastTriggerId == GesturePath.Count - 1)
                    SendMessage("GestureDetected");

                if (!UseGlobalTimer)
                    remainingTime = GesturePath[lastTriggerId].maxDelayToReachNextCollider;
            }
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        // Reset everything that could be in progress;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
            return;

        /////////////////////////
        // Start a new attempt //
        /////////////////////////

        // instantiate attempt
        Transform attemptTr = Instantiate(StepContainer, other.transform.position, Quaternion.Euler(Vector3.zero), transform);
        attemptTr.name = "GestureAttempt";
        GestureDetectionAttempt attempt = attemptTr.gameObject.AddComponent<GestureDetectionAttempt>();
        attempt.manager = this;

        // set listeners
        foreach (var step in attemptTr.GetComponentsInChildren<GestureDetectionStep>())
        {
            step.attempt = attempt;
        }

        // everything is ready ! start attempt;
        attemptTr.gameObject.SetActive(true);
    }
}
/*

[CustomEditor(typeof(GestureDetection))]
public class GestureDetectionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var myScript = target as GestureDetection;

        myScript.UseGlobalTimer = GUILayout.Toggle(myScript.UseGlobalTimer, "Use Global Timer");
        if(myScript.UseGlobalTimer)
            myScript.GlobalTimer = EditorGUILayout.FloatField(myScript.GlobalTimer, "Global Timer");

        EditorGUIUtility.LookLikeInspector();

/*
        EditorGUILayout.Foldout(true, "Gesture Path", true);
        foreach (var step in myScript.GesturePath)
        {
            //if (myScript.UseGlobalTimer)
            //  step.maxDelayToReachNextCollider = EditorGUILayout.IntSlider("I field:", myScript.i, 1, 100);
            step.collider = (Collider)EditorGUILayout.ObjectField(step.collider, typeof(Collider));
        }


    }
}
*/
