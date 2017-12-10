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

    public float SlideBackSpeed;

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
    void Update () {
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
                if (MenuContent.localPosition.x > 0)
                {
                    MenuContent.localPosition += Vector3.left * SlideBackSpeed;
                    if (MenuContent.localPosition.x <= 0)
                        currentState = eState.None;
                }
                else if (MenuContent.localPosition.x < 0)
                {
                    MenuContent.localPosition += Vector3.right * SlideBackSpeed;
                    if(MenuContent.localPosition.x >= 0)
                        currentState = eState.None;
                }
                else
                    currentState = eState.None;
                break;
        }
    }

    private Vector3 ComputeDeplacement(bool rightHanded)
    {
        Collider c = rightHanded ? RightStartClose : LeftStartClose;
        Vector3 mainLocal = c.transform.InverseTransformPoint(activeHand.position);

        float xDiff = Mathf.Ceil(mainLocal.x);
        if (xDiff <= 0 && rightHanded  || xDiff >= 0 && !rightHanded)
        {
            Vector3 pos = MenuContent.localPosition;
            pos += Vector3.right * xDiff;
            return pos;
        }
        else if(xDiff > 0 && rightHanded || xDiff < 0 && !rightHanded) 
        {
            Vector3 pos = MenuContent.localPosition;
            pos += Vector3.right * xDiff;
            if (rightHanded && pos.x > 0 || !rightHanded && pos.x < 0)
                return Vector3.zero;
            return pos;
        }
        else
            return Vector3.zero;
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
