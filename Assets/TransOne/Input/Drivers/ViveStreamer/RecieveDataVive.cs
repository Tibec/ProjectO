using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine.Networking;
using UnityEngine;



public class RecieveDataVive : RecieveData<ViveData> {

    static List<RecieveDataVive> instances = new List<RecieveDataVive>();

    public void Initialize(string ip, int port)
    {
        ipAddress = ip;
        socketPort = port;
        instances.Add(this);
    }

    void Start()
    {
        base.Init();
        data = new ViveData();
    }

    public static bool GetButton(string address,int id_button)
    {

        string[] tmp = address.Split(new char[] { '@' });
        uint index = Convert.ToUInt32(tmp[0]);
        string ip = tmp[1];

        RecieveDataVive r = instances.Find(x => x.ipAddress == ip);
        if(r != null)
        {
            DescriptionVive v = r.data.data.Find(x => x.controllerIndex == index);
            if(v!= null)
            {
                switch (id_button)
                {
                    case 0:return v.triggerPressed;
                    case 1:return v.menuPressed;
                    case 2:return v.steamPressed;
                    case 3:return v.gripped;
                    case 4:return v.padPressed;
                    case 5:return v.padTouched;
                   
                }

            }
        }
        return false;
    }

    public static float GetAxis(string address, int id_button)
    {

        string[] tmp = address.Split(new char[] { '@' });
        uint index = Convert.ToUInt32(tmp[0]);
        string ip = tmp[1];

        RecieveDataVive r = instances.Find(x => x.ipAddress == ip);
        if (r != null)
        {
            DescriptionVive v = r.data.data.Find(x => x.controllerIndex == index);
            if (v != null)
            {
                switch (id_button)
                {
                    case 0: return v.padX;
                    case 1: return v.padY;
                }

            }
        }
        return 0.0f;
    }


}
