using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class BasicInputViveStreamer : BasicInputTO
{

    public BasicInputViveStreamer() : base(){ }
    public BasicInputViveStreamer(int i, typeInput t) : base(i,t){ }


    public override bool GetButtonDown(string address, IConvertible nameButton)
    {

        if (type != BasicInputTO.typeInput.Analog)
        {
            tmp_isPressed = RecieveDataVive.GetButton(address, idInput);

        }
        else
        {
            tmp_isPressed = (RecieveDataVive.GetAxis(address, idInput) > 0.9f);
        }

        return (tmp_isPressed && !isPressed);

    }

    public override bool GetButtonUp(string address, IConvertible nameButton)
    {

        if (type != BasicInputTO.typeInput.Analog)
        {
            tmp_isPressed = RecieveDataVive.GetButton(address, idInput);
        }
        else
        {
            tmp_isPressed = (RecieveDataVive.GetAxis(address, idInput) > 0.9f);
        }

        return (!tmp_isPressed && isPressed);
    }




    public override bool GetButton(string address, IConvertible nameButton)
    {

        if (type != BasicInputTO.typeInput.Analog)
        {
			tmp_isPressed = RecieveDataVive.GetButton(address, idInput);
        }
        else
        {
			tmp_isPressed = (RecieveDataVive.GetAxis(address, idInput) > 0.9f);
        }
        return isPressed;

    }

    public override float GetAxis(string address, IConvertible nameButton)
    {


        if (type == BasicInputTO.typeInput.Analog)
        {
            return (float)RecieveDataVive.GetAxis(address, idInput);
        }
        return 0.0f;

    }

    public override Vector3 GetPosition(string address)
    {

        //Not implemented
        
        return Vector3.zero;

    }
    public override Quaternion GetRotation(string address)
    {

        //Not implemented
      
        return Quaternion.identity;

    }
}
