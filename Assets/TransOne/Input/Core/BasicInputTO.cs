using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// Abstract class to create Basic Inputs
/// A basic input links the corresponding button/axis/tracker of the device to Unity
/// Its the low level class
/// </summary>
abstract public class BasicInputTO {


	/// <summary>
	/// Basic event
	/// </summary>
    public class UpdateBasicInput : UnityEvent
    {}


	public enum typeInput
	{
		Analog,
		Button,
        Tracker,
		DPad
	}

    /// <summary>
    /// The ID of the button   
	/// </summary>
	protected int idInput;

	/// <summary>
	/// The subInput of the button
	/// For Dpads, 0: Up, 1:Right, 2:Down, 3:Left   
	/// </summary>

	protected int idSubInput;

	/// <summary>
	/// the current state of the button 
	/// </summary>
	protected bool isPressed;
	protected bool tmp_isPressed;
   
	protected typeInput type;
  /// <summary>
  /// Event to signal that the state of the button changed
/// </summary>
	public UpdateBasicInput ev;

	public BasicInputTO()
	{
		isPressed = false;
        ev = new UpdateBasicInput();
        ev.AddListener(UpdateIsPressed);
    }

	public BasicInputTO(int i,typeInput t)
	{
		type = t;
		idInput = i;
		isPressed = false;
        ev = new UpdateBasicInput();
        ev.AddListener(UpdateIsPressed);
		idSubInput = -1;
    }

	public BasicInputTO(int i,typeInput t,int j)
	{
		type = t;
		idInput = i;
		isPressed = false;
		ev = new UpdateBasicInput();
		ev.AddListener(UpdateIsPressed);
		idSubInput = j;
	}




    
    public void UpdateIsPressed()
	{
        isPressed = tmp_isPressed;
    }

	abstract public bool GetButtonDown (string address="",IConvertible nameButton = null);
	abstract public bool GetButtonUp (string address="",IConvertible nameButton = null);
	abstract public bool GetButton (string address="",IConvertible nameButton = null);
	abstract public float GetAxis (string address="",IConvertible nameButton = null);
	abstract public Vector3 GetPosition (string address="");
	abstract public Quaternion GetRotation (string address="");




}