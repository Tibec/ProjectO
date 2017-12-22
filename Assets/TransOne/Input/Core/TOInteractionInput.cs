using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TOInteractionInput
{


    public event Action OnInput;                                // Called when Fire1 is released and it's not a double click.
    public event Action OnInputDown;                                 // Called when Fire1 is pressed.
    public event Action OnInputUp;                                   // Called when Fire1 is released.
    public event Action OnDoubleInput;                          // Called when a double click is detected.

    public string nameInput;
    public int deviceID;
    public InputTypes typeInput;
    public string inputID;

    public bool debugMode;
    public KeyCode debugKey;
    [SerializeField]
    private float m_DoubleClickTime = 0.3f;    //The max time allowed between double clicks

    private float m_LastInputUpTime;                            // The time when Fire1 was last released.

    private Type type;

    public float DoubleClickTime { get { return m_DoubleClickTime; } }

    public void SetType()
    {

        if (typeInput == InputTypes.KeyCode)
            type = Type.GetType("UnityEngine.KeyCode,UnityEngine");
        else
            type = Type.GetType(typeInput.ToString());

    }




    public void CheckInput()
    {

        bool inputTest;

        if (debugMode)
            inputTest = Input.GetKeyDown(debugKey);
        else
            inputTest = TOInput.GetButtonDown((IConvertible)Enum.Parse(type, inputID), deviceID);

        if (inputTest)
        {

            // If anything has subscribed to OnDown call it.
            if (OnInputDown != null)
                OnInputDown();


        }



        // If there was no swipe this frame from the mouse, check for a keyboard swipe.
        /*
        if (swipe == SwipeDirection.NONE)
            swipe = DetectKeyboardEmulatedSwipe();

        // If there are any subscribers to OnSwipe call it passing in the detected swipe.
        if (OnSwipe != null)
            OnSwipe(swipe);*/


        if (debugMode)
            inputTest = Input.GetKeyUp(debugKey);
        else
            inputTest = TOInput.GetButtonUp((IConvertible)Enum.Parse(type, inputID), deviceID);

        // This if statement is to trigger events based on the information gathered before.
        if (inputTest)
        {
            // If anything has subscribed to OnUp call it.
            if (OnInputUp != null)
                OnInputUp();

            // If the time between the last release of Fire1 and now is less
            // than the allowed double click time then it's a double click.
            if (Time.time - m_LastInputUpTime < m_DoubleClickTime)
            {
                // If anything has subscribed to OnDoubleClick call it.
                if (OnDoubleInput != null)
                    OnDoubleInput();
            }


            // Record the time when Fire1 is released.
            m_LastInputUpTime = Time.time;
        }


        if (debugMode)
            inputTest = Input.GetKeyUp(debugKey);
        else
            inputTest = TOInput.GetButton((IConvertible)Enum.Parse(type, inputID), deviceID);
        if (inputTest)
        {
            if (OnInput != null)
                OnInput();
        }


    }

    /*
    private SwipeDirection DetectSwipe ()
    {
        // Get the direction from the mouse position when Fire1 is pressed to when it is released.
        Vector2 swipeData = (m_MouseUpPosition - m_MouseDownPosition).normalized;

        // If the direction of the swipe has a small width it is vertical.
        bool swipeIsVertical = Mathf.Abs (swipeData.x) < m_SwipeWidth;

        // If the direction of the swipe has a small height it is horizontal.
        bool swipeIsHorizontal = Mathf.Abs(swipeData.y) < m_SwipeWidth;

        // If the swipe has a positive y component and is vertical the swipe is up.
        if (swipeData.y > 0f && swipeIsVertical)
            return SwipeDirection.UP;

        // If the swipe has a negative y component and is vertical the swipe is down.
        if (swipeData.y < 0f && swipeIsVertical)
            return SwipeDirection.DOWN;

        // If the swipe has a positive x component and is horizontal the swipe is right.
        if (swipeData.x > 0f && swipeIsHorizontal)
            return SwipeDirection.RIGHT;

        // If the swipe has a negative x component and is vertical the swipe is left.
        if (swipeData.x < 0f && swipeIsHorizontal)
            return SwipeDirection.LEFT;

        // If the swipe meets none of these requirements there is no swipe.
        return SwipeDirection.NONE;
    }


    private SwipeDirection DetectKeyboardEmulatedSwipe ()
    {
        // Store the values for Horizontal and Vertical axes.
        float horizontal = Input.GetAxis ("Horizontal");
        float vertical = Input.GetAxis ("Vertical");

        // Store whether there was horizontal or vertical input before.
        bool noHorizontalInputPreviously = Mathf.Abs (m_LastHorizontalValue) < float.Epsilon;
        bool noVerticalInputPreviously = Mathf.Abs(m_LastVerticalValue) < float.Epsilon;

        // The last horizontal values are now the current ones.
        m_LastHorizontalValue = horizontal;
        m_LastVerticalValue = vertical;

        // If there is positive vertical input now and previously there wasn't the swipe is up.
        if (vertical > 0f && noVerticalInputPreviously)
            return SwipeDirection.UP;

        // If there is negative vertical input now and previously there wasn't the swipe is down.
        if (vertical < 0f && noVerticalInputPreviously)
            return SwipeDirection.DOWN;

        // If there is positive horizontal input now and previously there wasn't the swipe is right.
        if (horizontal > 0f && noHorizontalInputPreviously)
            return SwipeDirection.RIGHT;

        // If there is negative horizontal input now and previously there wasn't the swipe is left.
        if (horizontal < 0f && noHorizontalInputPreviously)
            return SwipeDirection.LEFT;

        // If the swipe meets none of these requirements there is no swipe.
        return SwipeDirection.NONE;
    }*/


    public void ResetEvents()
    {
        // Ensure that all events are unsubscribed when this is destroyed.
        //OnSwipe = null;
        OnInput = null;
        OnDoubleInput = null;
        OnInputDown = null;
        OnInputUp = null;
    }
}
