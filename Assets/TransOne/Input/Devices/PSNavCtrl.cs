using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PSNavCtrl : TOInput
{

	public PSNavCtrl(string name, string address) : base(name,address){}

	public static void Init<T>(int id,string name="", string address="" ) where T : BasicInputTO
	{
		PSNavCtrl n = new PSNavCtrl(name,address);

		n.inputs.Add (PSInputs.Cross, CreateInput<T>(0, BasicInputTO.typeInput.Button));
		n.inputs.Add (PSInputs.Circle, CreateInput<T>(1, BasicInputTO.typeInput.Button));
		n.inputs.Add (PSInputs.L1, CreateInput<T>(4, BasicInputTO.typeInput.Button));
		n.inputs.Add (PSInputs.L3, CreateInput<T>(8, BasicInputTO.typeInput.Button));
		n.inputs.Add (PSInputs.LeftAxisX, CreateInput<T>(0, BasicInputTO.typeInput.Analog));
		n.inputs.Add (PSInputs.LeftAxisY, CreateInput<T>(1, BasicInputTO.typeInput.Analog));
		n.inputs.Add (PSInputs.L2, CreateInput<T>(2, BasicInputTO.typeInput.Analog));

		n.inputs.Add (PSInputs.DPad, CreateInput<T>(4, BasicInputTO.typeInput.DPad));

		n.inputs.Add (PSInputs.Up, CreateInput<T>(4, BasicInputTO.typeInput.DPad,0));
		n.inputs.Add (PSInputs.Right, CreateInput<T>(4, BasicInputTO.typeInput.DPad,1));
		n.inputs.Add (PSInputs.Down, CreateInput<T>(4, BasicInputTO.typeInput.DPad,2));
		n.inputs.Add (PSInputs.Left, CreateInput<T>(4, BasicInputTO.typeInput.DPad,3));


		PSNavCtrl.instances.Add (id, n);
	}

    public static bool GetButtonDown(PSInputs nameButton,int id)
    {
        return A_GetButtonDown(nameButton, id);
    }

    public static bool GetButtonUp(PSInputs nameButton, int id)
    {
        return A_GetButtonUp(nameButton, id);
    }
    public static bool GetButton(PSInputs nameButton, int id)
    {
        return A_GetButton(nameButton, id);
    }
    public static float GetAxis(PSInputs nameButton, int id)
    {
        return A_GetAxis(nameButton, id);
    }
    
}
