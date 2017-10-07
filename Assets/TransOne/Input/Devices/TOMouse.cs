using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TOMouse : TOInput {


	public TOMouse(string name, string address) : base(name,address){}


	public static void Init<T>(int id,string name="", string address="" ) where T : BasicInputTO
	{
		TOMouse m = new TOMouse(name,address);

		m.inputs.Add (KeyCode.Mouse0, CreateInput<T>(0, BasicInputTO.typeInput.Button));
		m.inputs.Add (KeyCode.Mouse1,CreateInput<T>(1, BasicInputTO.typeInput.Button));
		m.inputs.Add (KeyCode.Mouse2,CreateInput<T>(2, BasicInputTO.typeInput.Button));
		m.inputs.Add (TOAxis.AxisX1,CreateInput<T>(0, BasicInputTO.typeInput.Analog));
		m.inputs.Add (TOAxis.AxisY1,CreateInput<T>(1, BasicInputTO.typeInput.Analog));

		TOMouse.instances.Add (id, m);

	}

    public static bool GetButtonDown(KeyCode nameButton, int id)
    {
        return A_GetButtonDown(nameButton, id);
    }

    public static bool GetButtonUp(KeyCode nameButton, int id)
    {
        return A_GetButtonUp(nameButton, id);
    }
    public static bool GetButton(KeyCode nameButton, int id)
    {
        return A_GetButton(nameButton, id);
    }
    public static float GetAxis(TOAxis nameButton, int id)
    {
        return A_GetAxis(nameButton, id);
    }

}
