using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine.Networking;
using UnityEngine;

[Serializable]
public class ConnectionData
{
    public int socketID, channelID, connectionID;

    public ConnectionData(int socket, int channel, int connection)
    {
        socketID = socket;
        channelID = channel;
        connectionID = connection;
    }
}



public class SendData<T> : MonoBehaviour {

    public int socketPort = 9994;
    public string customIpAddress = "";
    protected T data;
    List<ConnectionData> connectData = new List<ConnectionData>();

    int bufferSize = 1024;
    byte[] buffer;
    int recSocketId, recConnectionId, recChannelId;

    

   public void Init()
    {
        int channelID, socketID;
        NetworkTransport.Init();
        ConnectionConfig config = new ConnectionConfig();
        channelID = config.AddChannel(QosType.Reliable);
        if (customIpAddress != "")
            socketID = NetworkTransport.AddHost(new HostTopology(config, 10), socketPort, customIpAddress);
        else
            socketID = NetworkTransport.AddHost(new HostTopology(config, 10), socketPort);
        print("Socket open on " + socketPort);

        connectData.Add(new ConnectionData(socketID, channelID, 0));
    }



    public IEnumerator SendSocketMessage(T o, ConnectionData c)
    {

        bool sendData = true;
        while (sendData)
        {
            string mes = JsonUtility.ToJson(o);
            byte error;
            //print(data.Length);

            //System.Buffer.BlockCopy(mes.ToCharArray(), 0, buffer, 0 mes.Length * sizeof(char));
            buffer = new byte[bufferSize];
            Stream s = new MemoryStream(buffer);
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(s, mes);
            if (!NetworkTransport.Send(c.socketID, c.connectionID, c.channelID, buffer, bufferSize, out error))
            {
                print("Send error : " + (NetworkError)error);
                sendData = false;
            }
            else
            {
                if ((NetworkError)error != NetworkError.Ok)
                    print("Send error : " + (NetworkError)error);
            }

            yield return null;
        }
        yield return null;


    }


    public void UpdateData()
    {
        int dataSize;
        byte error;
        NetworkEventType recNetworkEvent = NetworkTransport.Receive(out recSocketId, out recConnectionId, out recChannelId, buffer, bufferSize, out dataSize, out error);
        //print((NetworkError)error);
        switch (recNetworkEvent)
        {
            case NetworkEventType.Nothing:
                break;
            case NetworkEventType.ConnectEvent:
                print("incoming connection");
                StartCoroutine(SendSocketMessage(data, new ConnectionData(recSocketId, recChannelId, recConnectionId)));
                break;
            default:
                break;
        }
    }

}
