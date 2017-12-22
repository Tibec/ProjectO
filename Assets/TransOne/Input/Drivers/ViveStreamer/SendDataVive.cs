using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[Serializable]
public class DescriptionVive
{
    public uint controllerIndex = 0;
    public bool triggerPressed = false;
    public bool steamPressed = false;
    public bool menuPressed = false;
    public bool padPressed = false;
    public bool padTouched = false;
    public bool gripped = false;
    public float padX = 0.0f;
    public float padY = 0.0f;

    public DescriptionVive()
    {
        controllerIndex = 0;
        triggerPressed = false;
        steamPressed = false;
        menuPressed = false;
        padPressed = false;
        padTouched = false;
        gripped = false;
        padX = 0.0f;
        padY = 0.0f;

    }

}


[Serializable]
public class ViveData
{
    public List<DescriptionVive> data = new List<DescriptionVive>();
}





public class SendDataVive : SendData<ViveData> {

   
   
    public ViveData controllerData;
    void Start()
    {
        

        data = controllerData;
        base.Init();
    }

    void Update()
    {
        for(int i = 0; i < controllerData.data.Count; i++)
        {
            StartCoroutine(UpdateController(i,controllerData.data[i].controllerIndex));

        }
       
        base.UpdateData();

    }




    IEnumerator UpdateController(int index,uint deviceIndex)
    {
        var system = OpenVR.System;
        VRControllerState_t controllerState = new VRControllerState_t();
        if (system != null && system.GetControllerState(deviceIndex, ref controllerState, (uint)System.Runtime.InteropServices.Marshal.SizeOf(typeof(VRControllerState_t))))
        {
            ulong trigger = controllerState.ulButtonPressed & (1UL << ((int)EVRButtonId.k_EButton_SteamVR_Trigger));
            if (trigger > 0L && !controllerData.data[index].triggerPressed)
            {
                controllerData.data[index].triggerPressed = true;
            }
            else if (trigger == 0L && controllerData.data[index].triggerPressed)
            {
                controllerData.data[index].triggerPressed = false;

            }

            ulong grip = controllerState.ulButtonPressed & (1UL << ((int)EVRButtonId.k_EButton_Grip));
            if (grip > 0L && !controllerData.data[index].gripped)
            {
                controllerData.data[index].gripped = true;
            }
            else if (grip == 0L && controllerData.data[index].gripped)
            {
                controllerData.data[index].gripped = false;
            }

            ulong pad = controllerState.ulButtonPressed & (1UL << ((int)EVRButtonId.k_EButton_SteamVR_Touchpad));
            if (pad > 0L && !controllerData.data[index].padPressed)
            {
                controllerData.data[index].padPressed = true;
            }
            else if (pad == 0L && controllerData.data[index].padPressed)
            {
                controllerData.data[index].padPressed = false;
            }

            ulong menu = controllerState.ulButtonPressed & (1UL << ((int)EVRButtonId.k_EButton_ApplicationMenu));
            if (menu > 0L && !controllerData.data[index].menuPressed)
            {
                controllerData.data[index].menuPressed = true;
            }
            else if (menu == 0L && controllerData.data[index].menuPressed)
            {
                controllerData.data[index].menuPressed = false;
            }

            pad = controllerState.ulButtonTouched & (1UL << ((int)EVRButtonId.k_EButton_SteamVR_Touchpad));
            if (pad > 0L && !controllerData.data[index].padTouched)
            {
                controllerData.data[index].padTouched = true;
            }
            else if (pad == 0L && controllerData.data[index].padTouched)
            {
                controllerData.data[index].padTouched = false;
            }

            controllerData.data[index].padX = controllerState.rAxis0.x;
            controllerData.data[index].padY = controllerState.rAxis0.y;

        }

        yield return null;
    }


}
