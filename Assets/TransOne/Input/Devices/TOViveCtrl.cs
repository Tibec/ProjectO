using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TOViveCtrl : TOInput
{
    
    public TOViveCtrl(string name, string address) : base(name,address){ }
    
    public static void Init<T>(int id, string name = "", string address = "") where T : BasicInputTO
    {
        TOViveCtrl n = new TOViveCtrl(id.ToString(), address);
        

        n.inputs.Add(ValveInputs.Trigger, CreateInput<T>(0, BasicInputTO.typeInput.Button));
        n.inputs.Add(ValveInputs.Menu, CreateInput<T>(1, BasicInputTO.typeInput.Button));
        n.inputs.Add(ValveInputs.Steam, CreateInput<T>(2, BasicInputTO.typeInput.Button));
        n.inputs.Add(ValveInputs.Grip, CreateInput<T>(3, BasicInputTO.typeInput.Button));
        n.inputs.Add(ValveInputs.PadPress, CreateInput<T>(4, BasicInputTO.typeInput.Button));
        n.inputs.Add(ValveInputs.PadTouch, CreateInput<T>(5, BasicInputTO.typeInput.Button));
        n.inputs.Add(ValveInputs.PadX, CreateInput<T>(0, BasicInputTO.typeInput.Analog));
        n.inputs.Add(ValveInputs.PadY, CreateInput<T>(1, BasicInputTO.typeInput.Analog));
        TOViveCtrl.instances.Add(id, n);
    }
    
    public static bool GetButtonDown(ValveInputs nameButton, int id)
    {
        return A_GetButtonDown(nameButton, id);
    }

    public static bool GetButtonUp(ValveInputs nameButton, int id)
    {
        return A_GetButtonUp(nameButton, id);
    }
    public static bool GetButton(ValveInputs nameButton, int id)
    {
        return A_GetButton(nameButton, id);
    }
    public static float GetAxis(ValveInputs nameButton, int id)
    {
        return A_GetAxis(nameButton, id);
    }
}
