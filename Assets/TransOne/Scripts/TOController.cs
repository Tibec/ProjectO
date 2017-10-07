//Note : this script is launched before all others
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Threading;
using UnityEngine.VR;
using UnityEngine.Networking;

[Serializable]
public class EditorParameters{
	public bool enableEditorParameters=false,isServer=false,isClient =false;
	public string ip_server,ip_client;
	public int port;
	public int[] id_displays;

}



public class TOController : MonoBehaviour {


    public static TOController GetInstance
    {
        get
        {
            return Singleton<TOController>.instance;
        }

    }
    public string fileDisplay;
    public string fileNodes;
    public string fileTracker;
	public EditorParameters editorParameters;
	//TODO Not working in stereo for some reason
    //public int forceFramerate = 120;

    void Awake()
    {

		System.GC.Collect();
		System.GC.WaitForPendingFinalizers();
		// Second call to work around a possible mono bug (see https://bugzilla.xamarin.com/show_bug.cgi?id=20503 )
		System.GC.WaitForPendingFinalizers();

        if (Singleton<TOController>.CheckSingletonExists(this)) return;


        //TODO Not working in stereo for some reason
        //if (forceFramerate != 0) Application.targetFrameRate = forceFramerate;

#if UNITY_EDITOR

        if (editorParameters.enableEditorParameters){
			TOParameters.isClient = editorParameters.isClient;
			TOParameters.isServer = editorParameters.isServer;
			TOParameters.id_displays = editorParameters.id_displays;
			TOParameters.ip_client = editorParameters.ip_client;
			TOParameters.ip_server = editorParameters.ip_server;
			TOParameters.port = editorParameters.port;
		}
#else
        TOParameters.InitTOParametersStandalone();
#endif

        TOParameters.displayParameters = new DisplayParameters();
        TOParameters.displayParameters.displays = new List<TODisplay>();
        if (fileDisplay != "")
        {
			TOParameters.LoadTOParameters(ParameterType.Display, Path.Combine(Application.streamingAssetsPath,fileDisplay));
        }

        TOParameters.nodesParameters = new TONodeParameters();
        TOParameters.nodesParameters.root = new TONode();
        TOParameters.nodesParameters.planes = new List<TOPlane>();
        if (fileNodes != "")
        {
			TOParameters.LoadTOParameters(ParameterType.Nodes, Path.Combine(Application.streamingAssetsPath,fileNodes));
        }

        TOParameters.trackerParameters = new TrackerParameters();
        TOParameters.trackerParameters.trackerServers = new List<TOTrackerServer>();
        if (fileTracker != "")
        {
			TOParameters.LoadTOParameters(ParameterType.Tracker, Path.Combine(Application.streamingAssetsPath,fileTracker));
           
        }
		if (TOParameters.isClient || TOParameters.isServer)
			GetComponent<NetworkManager> ().enabled = true;

		
    }


}
