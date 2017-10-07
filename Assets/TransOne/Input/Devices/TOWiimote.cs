using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TOWiimote : TOInput {

	public TOWiimote(string name, string address) : base(name,address){}

	public static void Init<T>(int id,string name="", string address="" ) where T : BasicInputTO
	{
		TOWiimote n = new TOWiimote(name,address);

		n.inputs.Add (WiiInputs.A, CreateInput<T>(0, BasicInputTO.typeInput.Button));
		n.inputs.Add (WiiInputs.B, CreateInput<T>(1, BasicInputTO.typeInput.Button));
		n.inputs.Add (WiiInputs.C, CreateInput<T>(4, BasicInputTO.typeInput.Button));
		n.inputs.Add (WiiInputs.Z, CreateInput<T>(5, BasicInputTO.typeInput.Button));
		n.inputs.Add (WiiInputs.Plus, CreateInput<T>(7, BasicInputTO.typeInput.Button));
		n.inputs.Add (WiiInputs.Minus, CreateInput<T>(6, BasicInputTO.typeInput.Button));
		n.inputs.Add (WiiInputs.One, CreateInput<T>(2, BasicInputTO.typeInput.Button));
		n.inputs.Add (WiiInputs.Two, CreateInput<T>(3, BasicInputTO.typeInput.Button));
		n.inputs.Add (WiiInputs.Home, CreateInput<T>(9, BasicInputTO.typeInput.Button));

		n.inputs.Add (WiiInputs.LeftAxisX, CreateInput<T>(0, BasicInputTO.typeInput.Analog));
		n.inputs.Add (WiiInputs.LeftAxisY, CreateInput<T>(1, BasicInputTO.typeInput.Analog));
		n.inputs.Add (WiiInputs.DPad, CreateInput<T>(4, BasicInputTO.typeInput.DPad));

		n.inputs.Add (WiiInputs.Up, CreateInput<T>(4, BasicInputTO.typeInput.DPad,0));
		n.inputs.Add (WiiInputs.Right, CreateInput<T>(4, BasicInputTO.typeInput.DPad,1));
		n.inputs.Add (WiiInputs.Down, CreateInput<T>(4, BasicInputTO.typeInput.DPad,2));
		n.inputs.Add (WiiInputs.Left, CreateInput<T>(4, BasicInputTO.typeInput.DPad,3));



		TOWiimote.instances.Add (id, n);
	}

	public static bool GetButtonDown(WiiInputs nameButton,int id)
	{
		return A_GetButtonDown(nameButton, id);
	}

	public static bool GetButtonUp(WiiInputs nameButton, int id)
	{
		return A_GetButtonUp(nameButton, id);
	}
	public static bool GetButton(WiiInputs nameButton, int id)
	{
		return A_GetButton(nameButton, id);
	}
	public static float GetAxis(WiiInputs nameButton, int id)
	{
		return A_GetAxis(nameButton, id);
	}

}
