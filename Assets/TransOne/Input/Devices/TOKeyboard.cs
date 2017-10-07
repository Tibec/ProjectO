using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class TOKeyboard : TOInput
{

    [DllImport("user32.dll")]
    private static extern uint MapVirtualKey(uint i,uint j);

	public TOKeyboard(string name, string address) : base(name,address){}

	public static void Init<T>(int id,string name="", string address="" ) where T : BasicInputTO
	{


		TOKeyboard k = new TOKeyboard(name,address);

		if (typeof(T) == typeof(BasicInputVRPN)) {

			for(int i = 0; i < 256; i++)
			{
				KeyCodeVRPN tmp = (KeyCodeVRPN)i;
				k.inputs.Add(tmp, CreateInput<T>((int)MapVirtualKey((uint)i, 0), BasicInputTO.typeInput.Button));
			}

		}
		if (typeof(T) == typeof(BasicInputWindows)) {
			for(int i = 0; i < 330; i++)
			{
				k.inputs.Add((KeyCode)i, CreateInput<T>(i, BasicInputTO.typeInput.Button));
			}
		}

		TOKeyboard.instances.Add (id, k);


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



    /// <summary>
    /// Use this with VRPN
    /// </summary>
    public static bool GetButtonDown(KeyCodeVRPN nameButton, int id)
    {
        return A_GetButtonDown(nameButton, id);
    }

    /// <summary>
    /// Use this with VRPN
    /// </summary>
    public static bool GetButtonUp(KeyCodeVRPN nameButton, int id)
    {
        return A_GetButtonUp(nameButton, id);
    }
    /// <summary>
    /// Use this with VRPN
    /// </summary>
    public static bool GetButton(KeyCodeVRPN nameButton, int id)
    {
        return A_GetButton(nameButton, id);
    }




}
