using System;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;
using UnityEngine;

public class BasicInputWindows : BasicInputTO {


	public int index=-1;
	public BasicInputWindows() : base(){}
	public BasicInputWindows(int i,typeInput t) : base(i,t){}



	public override bool GetButtonDown(string address,IConvertible nameButton)
	{
		if(address == "Mouse" || address == "Keyboard")
			return Input.GetKeyDown ((KeyCode)nameButton);


		if(index ==-1)
			index = Convert.ToInt32 (address.Split (new char []{ '@' }) [1]);

		if (type != BasicInputTO.typeInput.Analog)
		{
			tmp_isPressed = (GetButtonFromIndex (index) == ButtonState.Pressed);
		}
		else
		{
			tmp_isPressed = (GetAxisFromIndex (index) > 0.9f);
		}
		return (tmp_isPressed && !isPressed);


	}

	public override bool GetButtonUp(string address,IConvertible nameButton)
	{

		if(address == "Mouse" || address == "Keyboard")
			return Input.GetKeyUp ((KeyCode)nameButton);

		if(index ==-1)
			index = Convert.ToInt32 (address.Split (new char []{ '@' }) [1]);

		if (type != BasicInputTO.typeInput.Analog)
		{
			tmp_isPressed = (GetButtonFromIndex (0) == ButtonState.Pressed);
		}
		else
		{
			tmp_isPressed = (GetAxisFromIndex (0) > 0.9f);
		}
		return (!tmp_isPressed && isPressed);


	}

	public override bool GetButton(string address,IConvertible nameButton)
	{

		if(address == "Mouse" || address == "Keyboard")
			return Input.GetKey ((KeyCode)nameButton);

		if(index ==-1)
			index = Convert.ToInt32 (address.Split (new char []{ '@' }) [1]);


		if (type != BasicInputTO.typeInput.Analog)
		{
			tmp_isPressed = (GetButtonFromIndex (index) == ButtonState.Pressed);
		}
		else
		{
			tmp_isPressed = (GetAxisFromIndex (index) > 0.9f);
		}
		return isPressed;

	}

	public override float GetAxis(string address,IConvertible nameButton)
	{
		if (address == "Mouse") {
			if ((TOAxis)nameButton == TOAxis.AxisX1)
				return Input.mousePosition.x;
			if ((TOAxis)nameButton == TOAxis.AxisY1)
				return Input.mousePosition.y;
		}

		if(index ==-1)
			index = Convert.ToInt32 (address.Split (new char []{ '@' }) [1]);

		return GetAxisFromIndex (index);
	}

	public ButtonState GetButtonFromIndex(int index){


		GamePadState s = GamePad.GetState ((PlayerIndex)index);

		switch (this.idInput) 
		{
		case 0:return s.Buttons.A;
		case 1:return s.Buttons.B;
		case 2:return s.Buttons.X;
		case 3:return s.Buttons.Y;
		case 4:return s.Buttons.LeftShoulder;
		case 5:return s.Buttons.RightShoulder;
		case 6:return s.Buttons.Back;
		case 7:return s.Buttons.Start;
		case 8:return s.Buttons.LeftStick;
		case 9:return s.Buttons.RightStick;
		case 10:return s.DPad.Left;
		case 11:return s.DPad.Right;
		case 12:return s.DPad.Up;
		case 13:return s.DPad.Down;
		default : return ButtonState.Released;

		}

	}

	public float GetAxisFromIndex(int index){

		GamePadState s = GamePad.GetState ((PlayerIndex)index);

		switch (this.idInput) 
		{
		case 0:return s.ThumbSticks.Left.X;
		case 1:return s.ThumbSticks.Left.Y;
		case 2:return s.Triggers.Left;
		case 3:return s.Triggers.Right;
		case 4:return s.ThumbSticks.Right.X;
		case 5:return s.ThumbSticks.Right.Y;
		default : return 0.0f;
		}

	}



	public override Vector3 GetPosition(string address)
	{
		return Vector3.zero;
	}
	public override Quaternion GetRotation(string address)
	{
		return Quaternion.identity;
	}

}
