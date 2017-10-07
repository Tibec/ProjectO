using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class BasicInputVRPN : BasicInputTO {


	public BasicInputVRPN() : base(){}
	public BasicInputVRPN(int i,typeInput t) : base(i,t){}
	public BasicInputVRPN(int i,typeInput t,int j) : base(i,t,j){}


	public override bool GetButtonDown(string address,IConvertible nameButton)
	{
		GetValueButton (address);
		return (tmp_isPressed && !isPressed);
	}

	public override bool GetButtonUp(string address,IConvertible nameButton)
	{
		GetValueButton (address);
		return (!tmp_isPressed && isPressed);
	}
		
	public override bool GetButton(string address,IConvertible nameButton)
	{
		GetValueButton (address);
		return isPressed;
	}

	private void GetValueButton(string address)
	{
		if (type == typeInput.Analog) { 
			tmp_isPressed = (VRPN.vrpnAnalog (address, idInput) > 0.9f);
		}
		else if (type == typeInput.DPad) {
			double value = VRPN.vrpnAnalog (address, idInput);
			bool direction = true;
			if(idSubInput!=-1)
				direction = ( value == idSubInput*90) || ( value == (idSubInput*90+45)%360) || ( value == (idSubInput*90-45)%360);
			tmp_isPressed = direction && (value != -1.0f);
		}
		else 
			tmp_isPressed = VRPN.vrpnButton(address, idInput);
	}

	public override float GetAxis(string address,IConvertible nameButton)
	{
		
		if ((type ==typeInput.Analog) || (type ==typeInput.DPad))
			return (float)VRPN.vrpnAnalog(address, idInput);

		return 0.0f;

	}

	public override Vector3 GetPosition(string address)
	{
		if (type == typeInput.Tracker)
			return VRPN.vrpnTrackerPos(address, idInput);
		return Vector3.zero;
	}
	public override Quaternion GetRotation(string address)
	{
		if (type ==typeInput.Tracker)
			return VRPN.vrpnTrackerQuat(address, idInput);
		return Quaternion.identity;
	}

}
