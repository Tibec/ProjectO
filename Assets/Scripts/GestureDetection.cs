using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class GestureStep
{
    public Collider collider;
    [Tooltip("Temps en secondes, ignoré si UseGlobalTimer = true")]
    public float maxDelayToReachNextCollider;
}

public class GestureDetection : MonoBehaviour {

    public string name;

    public bool UseGlobalTimer;
    public float GlobalTimer;
    [SerializeField]
    public List<GestureStep> GesturePath;

    private int lastTriggerId = -1;
    private float remainingTime = 0;

	// Use this for initialization
	void Start ()
    {
        lastTriggerId = -1;

        foreach (var step in GesturePath)
        {
            CollisionListener cl;
            cl = step.collider.gameObject.AddComponent<CollisionListener>();
            cl.listener = this.gameObject;
        }
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (lastTriggerId == -1)
            return;

        remainingTime -= Time.fixedDeltaTime;

        if (remainingTime <= 0)
            lastTriggerId = -1;

    }

    void OnRemoteCollisionEnter(CollisionListenerData data)
    {
        if (lastTriggerId + 1 > GesturePath.Count)
            return;

        if (data.sender == GesturePath[lastTriggerId + 1].collider.gameObject)
        {
            if(data.collision.tag == "Player")
            {
                ++lastTriggerId;

                if (lastTriggerId == 0)
                    remainingTime = GlobalTimer;

                if (lastTriggerId == GesturePath.Count - 1)
                    SendMessage("GestureDetected", name);

                if (!UseGlobalTimer)
                    remainingTime = GesturePath[lastTriggerId].maxDelayToReachNextCollider;
            }
        }
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