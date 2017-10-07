using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TOGenericVRPN : TOInput
{

    public TOGenericVRPN(string name, string address) : base(name,address){ }

    public static void Init<T>(int id, string name = "", string address = "") where T : BasicInputTO
    {
        TOGenericVRPN n = new TOGenericVRPN(name, address);

        for(int i=0;i<19;i++)
            n.inputs.Add((TOButtons)i, CreateInput<T>(i, BasicInputTO.typeInput.Button));

        for (int i = 0; i < 12; i++)
            n.inputs.Add((TOAxis)i, CreateInput<T>(i, BasicInputTO.typeInput.Analog));


        n.inputs.Add("", CreateInput<T>(0, BasicInputTO.typeInput.Tracker));

        TOGenericVRPN.instances.Add(id, n);
    }

    public static bool GetButtonDown(TOButtons nameButton, int id)
    {
        return A_GetButtonDown(nameButton, id);
    }

    public static bool GetButtonUp(TOButtons nameButton, int id)
    {
        return A_GetButtonUp(nameButton, id);
    }
    public static bool GetButton(TOButtons nameButton, int id)
    {
        return A_GetButton(nameButton, id);
    }
    public static float GetAxis(TOAxis nameButton, int id)
    {
        return A_GetAxis(nameButton, id);
    }

    public static Vector3 GetPosition(int id)
    {
        return A_GetPosition("", id);
    }

    public static Quaternion GetRotation(int id)
    {
        return A_GetRotation("", id);
    }

}
