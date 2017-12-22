using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This class should be added to any gameobject in the scene
// that should react to input based on the user's gaze.
// It contains events that can be subscribed to by classes that
// need to know about input specifics to this gameobject.
public class TOInteractiveItem : MonoBehaviour
{

    public event Action<TORaycaster> OnOver;             // Called when the gaze moves over this object
    public event Action<TORaycaster> OnOut;              // Called when the gaze leaves this object
    public event Action<TORaycaster> OnInput;            // Called when click input is detected whilst the gaze is over this object.
    public event Action<TORaycaster> OnDoubleInput;      // Called when double click input is detected whilst the gaze is over this object.
    public event Action<TORaycaster> OnUp;               // Called when Fire1 is released whilst the gaze is over this object.
    public event Action<TORaycaster> OnDown;             // Called when Fire1 is pressed whilst the gaze is over this object.


    protected bool m_IsOver;


    public bool IsOver
    {
        get { return m_IsOver; }              // Is the gaze currently over this object?
    }


    // The below functions are called by the VREyeRaycaster when the appropriate input is detected.
    // They in turn call the appropriate events should they have subscribers.
    public void Over(TORaycaster raycaster)
    {
        m_IsOver = true;

        if (OnOver != null)
            OnOver(raycaster);
    }


    public void Out(TORaycaster raycaster)
    {
        m_IsOver = false;

        if (OnOut != null)
            OnOut(raycaster);
    }


    public void Click(TORaycaster raycaster)
    {
        if (OnInput != null)
            OnInput(raycaster);
    }


    public void DoubleClick(TORaycaster raycaster)
    {
        if (OnDoubleInput != null)
            OnDoubleInput(raycaster);
    }


    public void Up(TORaycaster raycaster)
    {
        if (OnUp != null)
            OnUp(raycaster);
    }


    public void Down(TORaycaster raycaster)
    {
        if (OnDown != null)
            OnDown(raycaster);
    }
}
