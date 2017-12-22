using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine.Networking;
using UnityEngine;

public class RecieveData<T> : MonoBehaviour {

    public string ipAddress = "127.0.0.1";
    public int socketPort = 9993;

    public T data;
    int socketID;

    int bufferSize = 1024;
    byte[] buffer;
    char[] res;
    // Use this for initialization

    public void Init()
    {
        NetworkTransport.Init();
        ConnectionConfig config = new ConnectionConfig();
        config.AddChannel(QosType.Reliable);

        socketID = NetworkTransport.AddHost(new HostTopology(config, 10));
        print("Socket open");
        Connect();
    }

    

    public void Connect()
    {
        byte error;
        NetworkTransport.Connect(socketID, ipAddress, socketPort, 0, out error);
        print((NetworkError)error);

    }

    // Update is called once per frame
    void Update()
    {
        int recSocketId, recConnectionId, recChannelId;
        int dataSize;
        byte error;
        buffer = new byte[bufferSize];
        NetworkEventType recNetworkEvent = NetworkTransport.Receive(out recSocketId, out recConnectionId, out recChannelId, buffer, bufferSize, out dataSize, out error);
        //print((NetworkError)error);
        switch (recNetworkEvent)
        {
            case NetworkEventType.Nothing:
                break;
            case NetworkEventType.ConnectEvent:
                print("incoming connection");
                break;
            case NetworkEventType.DataEvent:
                Stream s = new MemoryStream(buffer);
                BinaryFormatter b = new BinaryFormatter();
                string mes = b.Deserialize(s) as string;
                data = JsonUtility.FromJson<T>(mes);

                break;
            default:
                break;
        }

    }
}
