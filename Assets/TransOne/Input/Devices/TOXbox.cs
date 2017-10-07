using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TOXbox : TOInput {

	public TOXbox(string name, string address) : base(name,address){}

	public static void Init<T>(int id,string name="", string address="" ) where T : BasicInputTO
	{
		TOXbox n = new TOXbox(name,address);

		n.inputs.Add (XboxInputs.A, CreateInput<T>(0, BasicInputTO.typeInput.Button));
		n.inputs.Add (XboxInputs.B, CreateInput<T>(1, BasicInputTO.typeInput.Button));
		n.inputs.Add (XboxInputs.X, CreateInput<T>(2, BasicInputTO.typeInput.Button));
		n.inputs.Add (XboxInputs.Y, CreateInput<T>(3, BasicInputTO.typeInput.Button));
		n.inputs.Add (XboxInputs.RB, CreateInput<T>(4, BasicInputTO.typeInput.Button));

		n.inputs.Add (XboxInputs.RB, CreateInput<T>(5, BasicInputTO.typeInput.Button));
		n.inputs.Add (XboxInputs.Start, CreateInput<T>(7, BasicInputTO.typeInput.Button));
		n.inputs.Add (XboxInputs.Select, CreateInput<T>(6, BasicInputTO.typeInput.Button));
		n.inputs.Add (XboxInputs.L3, CreateInput<T>(8, BasicInputTO.typeInput.Button));
		n.inputs.Add (XboxInputs.R3, CreateInput<T>(9, BasicInputTO.typeInput.Button));

		n.inputs.Add (XboxInputs.LeftAxisX, CreateInput<T>(0, BasicInputTO.typeInput.Analog));
		n.inputs.Add (XboxInputs.LeftAxisY, CreateInput<T>(1, BasicInputTO.typeInput.Analog));
		n.inputs.Add (XboxInputs.Trigger, CreateInput<T>(2, BasicInputTO.typeInput.Analog));
		n.inputs.Add (XboxInputs.RightAxisX, CreateInput<T>(4, BasicInputTO.typeInput.Analog));
		n.inputs.Add (XboxInputs.RightAxisY, CreateInput<T>(3, BasicInputTO.typeInput.Analog));

		n.inputs.Add (XboxInputs.DPad, CreateInput<T>(8, BasicInputTO.typeInput.Analog));
		n.inputs.Add (XboxInputs.Up, CreateInput<T>(8, BasicInputTO.typeInput.DPad,0));
		n.inputs.Add (XboxInputs.Right, CreateInput<T>(8, BasicInputTO.typeInput.DPad,1));
		n.inputs.Add (XboxInputs.Down, CreateInput<T>(8, BasicInputTO.typeInput.DPad,2));
		n.inputs.Add (XboxInputs.Left, CreateInput<T>(8, BasicInputTO.typeInput.DPad,3));

		TOXbox.instances.Add (id, n);
	}

	public static bool GetButtonDown(XboxInputs nameButton,int id)
	{
		return A_GetButtonDown(nameButton, id);
	}

	public static bool GetButtonUp(XboxInputs nameButton, int id)
	{
		return A_GetButtonUp(nameButton, id);
	}
	public static bool GetButton(XboxInputs nameButton, int id)
	{
		return A_GetButton(nameButton, id);
	}
	public static float GetAxis(XboxInputs nameButton, int id)
	{
		return A_GetAxis(nameButton, id);
	}


}
