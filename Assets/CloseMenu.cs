using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseMenu : MonoBehaviour {

    enum eState { None, FollowRight, FollowLeft, SlideBack };

    public Collider LeftStartClose;
    public Collider RightStartClose;
    public Collider EndClose;

    public Transform MenuContent;

    private Transform activeHand;
    private eState currentState = eState.None;

    // Use this for initialization
    void Start () {
        CollisionListener cl;
        cl = LeftStartClose.gameObject.AddComponent<CollisionListener>();
        cl.listener = this.gameObject;
        cl = RightStartClose.gameObject.AddComponent<CollisionListener>();
        cl.listener = this.gameObject;
        cl = EndClose.gameObject.AddComponent<CollisionListener>();
        cl.listener = this.gameObject;
    }

    // Update is called once per frame
    void LateUpdate () {
		switch(currentState)
        {
            case eState.None:
                break;
            case eState.FollowRight:
                MenuContent.localPosition = ComputeDeplacement(true);
                break;
            case eState.FollowLeft:
                MenuContent.localPosition = ComputeDeplacement(false);
                break;
            case eState.SlideBack:
                MenuContent.localPosition = Vector3.zero;
                currentState = eState.None;
                break;
        }
    }

    private Vector3 ComputeDeplacement(bool rightHanded)
    {
        Collider c = rightHanded ? RightStartClose : LeftStartClose;
        Vector3 mainLocal = c.transform.InverseTransformVector(activeHand.position);

        float xDiff = c.transform.localPosition.x - mainLocal.x;
        if (xDiff > 0 && rightHanded ) // La main avance depuis la gauche
        {
            Vector3 pos = transform.localPosition;
            pos += Vector3.left * xDiff;
            return pos;
        }
        else if(xDiff < 0 && !rightHanded) // La main avance depuis la droite
        {
            Vector3 pos = transform.localPosition;
            pos -= Vector3.right * xDiff;
            return pos;
        }
        else
            return transform.localPosition;
    }

    void OnRemoteCollisionEnter(CollisionListenerData data)
    {
        if (data.sender == LeftStartClose.gameObject && currentState == eState.None)
        {
            currentState = eState.FollowLeft;
            activeHand = data.collision.transform;
        }
        if (data.sender == RightStartClose.gameObject && currentState == eState.None)
        {
            currentState = eState.FollowRight;
            activeHand = data.collision.transform;
        }
        if (data.sender == EndClose.gameObject 
            && (currentState == eState.FollowLeft || currentState == eState.FollowRight))
        {
            gameObject.SetActive(false);
        }
    }

    void OnRemoteCollisionExit(CollisionListenerData data)
    {
        if (data.sender == LeftStartClose.gameObject && currentState == eState.FollowLeft)
        {
            currentState = eState.SlideBack;
        }
        if (data.sender == RightStartClose.gameObject && currentState == eState.FollowRight)
        {
            currentState = eState.SlideBack;
        }
    }

}
