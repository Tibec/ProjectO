using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;


static class TOParameters {

	//Start parameters
	public static DisplayParameters displayParameters;
	public static TrackerParameters trackerParameters;
	public static TONodeParameters nodesParameters;


	//Current instance parameters
	public static bool isServer=false,isClient =false;
	public static string ip_server,ip_client;
	public static int port;
	public static int[] id_displays;
 
	/// <summary>
	/// Initialize current instance parameters in a standalone application
	/// </summary>
    public static void InitTOParametersStandalone()
	{

		string file_tmp = ReadCommandLineArgs("+displayFile", 1);
		if (file_tmp != null) {
            TOController.GetInstance.fileDisplay = file_tmp;
		}

        file_tmp = ReadCommandLineArgs("+nodeFile", 1);
        if (file_tmp != null)
        {
            TOController.GetInstance.fileNodes = file_tmp;
        }

        file_tmp = ReadCommandLineArgs("+trackerFile", 1);
        if (file_tmp != null)
        {
            TOController.GetInstance.fileTracker = file_tmp;
        }
       
		isServer = (ReadCommandLineArgs("+server",0) !=null);
		isClient = (ReadCommandLineArgs("+client",0) !=null);

		string s_displays = ReadCommandLineArgs("+display", 1); 


		if(s_displays!=null){
			string[] tmp_displays = s_displays.Split(new char[] { ',' });
			id_displays = new int[tmp_displays.Length];
			for (int i = 0; i < tmp_displays.Length; i++) id_displays[i] = Convert.ToInt32(tmp_displays[i]); 
		}

        if (isServer)
        {
            ip_server = ReadCommandLineArgs("+ipServer", 1);
			ip_client = ip_server;
            port = Convert.ToInt32(ReadCommandLineArgs("+port", 1));

        }

		if (isClient)
		{
			ip_server = ReadCommandLineArgs("+ipServer", 1);
			ip_client = ReadCommandLineArgs("+ipClient", 1);
			port = Convert.ToInt32(ReadCommandLineArgs("+port", 1));

		}

    }



	/// <summary>
	/// Initialize all input/output devices
	/// </summary>
    public static void InitTrackers()
    {
        for(int i = 0; i < trackerParameters.trackerServers.Count; i++)
        {
            switch (trackerParameters.trackerServers[i].type)
            {

            case TOTrackerServerType.VRPN :
				for(int j=0;j< trackerParameters.trackerServers[i].trackers.Count; j++)
                {
                    string ip = trackerParameters.trackerServers[i].ip;
                    //int port = trackerParameters.trackerServers[i].port;
                    string name = trackerParameters.trackerServers[i].trackers[j].name;
                    int id = trackerParameters.trackerServers[i].trackers[j].id;


                    switch (trackerParameters.trackerServers[i].trackers[j].type)
                    {
						case TOTrackerType.NavCtrl: PSNavCtrl.Init<BasicInputVRPN>(id,name,ip); break;
						case TOTrackerType.Wiimote: TOWiimote.Init<BasicInputVRPN> (id, name, ip);break;
						case TOTrackerType.Mouse: TOMouse.Init<BasicInputVRPN>(id,name,ip); break;
					    case TOTrackerType.Keyboard: TOKeyboard.Init<BasicInputVRPN>(id,name,ip); break;
                        case TOTrackerType.GenericVRPN: TOGenericVRPN.Init<BasicInputVRPN>(id, name, ip);break;
						case TOTrackerType.Xbox: TOXbox.Init<BasicInputVRPN>(id, name, ip);break;
                    }
				TOInputController.UpdateData(id);
                }
				break;

			case TOTrackerServerType.NoServer:
				for (int j = 0; j < trackerParameters.trackerServers [i].trackers.Count; j++) {
					
					int id = trackerParameters.trackerServers [i].trackers [j].id;
					switch (trackerParameters.trackerServers [i].trackers [j].type) {
					case TOTrackerType.NavCtrl:
						PSNavCtrl.Init<BasicInputWindows> (id, "PSInputs", id.ToString()); //via SCPToolkit
						break;
					case TOTrackerType.Mouse:
						TOMouse.Init<BasicInputWindows> (id, "Mouse", "");
						break;
					case TOTrackerType.Keyboard:
						TOKeyboard.Init<BasicInputWindows> (id, "Keyboard", "");

						break;
					}
					TOInputController.UpdateData(id);

				}
                    break;

                case TOTrackerServerType.Vive:


                    string ip2 = trackerParameters.trackerServers[i].ip;
                    int port = trackerParameters.trackerServers[i].port;
                    TOInputController.GetInstance.gameObject.AddComponent<RecieveDataVive>().Initialize(ip2, port);
                    for (int j = 0; j < trackerParameters.trackerServers[i].trackers.Count; j++)
                    {
                        int id = trackerParameters.trackerServers[i].trackers[j].id;
                        TOViveCtrl.Init<BasicInputViveStreamer>(id, "", ip2);
                    }

                        break;
                    	

				//TODO Implement Optitrack and others inside TOInput
                    /*
                case TOTrackerServerType.Optitrack:
                    GameObject serv = new GameObject(trackerParameters.trackerServers[i].name);
                    OptitrackStreamingClient os = serv.AddComponent<OptitrackStreamingClient>();
                    os.ServerAddress = trackerParameters.trackerServers[i].ip;
                    os.LocalAddress = "localhost";
                    os.ServerCommandPort = 1510;
                    os.ServerDataPort = (ushort)trackerParameters.trackerServers[i].port;

                    for (int j = 0; j < trackerParameters.trackerServers[i].trackers.Count; j++)
                    {
                       
                        switch (trackerParameters.trackerServers[i].trackers[j].type)
                        {
                            case TOTrackerType.OptitrackRB:
                               

                                    break;
                        }
                    }*/
				
                   
            }
        }

    }


	/// <summary>
	/// Read Command line arguments in the application
	/// </summary>
	/// <returns>The command line arguments.</returns>
	/// <param name="cmd">argument prefix</param>
	/// <param name="index">index of the argument</param>
	public static string ReadCommandLineArgs(string cmd,int index){
		string[] args = System.Environment.GetCommandLineArgs ();
		for (int i = 0; i < args.Length; i++) 
		{
			if (args [i] == cmd) return args [i + index];
		}
		return null;
	}

	/// <summary>
	/// Load a config file. if no file is specified, a window dialog is open
	/// </summary>
	/// <param name="t">Type</param>
	/// <param name="file">Fil.</param>
	public static void LoadTOParameters(ParameterType t,string file=null)
    {
        if (file == null)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = string.Format("{0} configuration file|*.{0}.json", t);
            openFileDialog1.Title = string.Format("Choose {0} configuration file",t);

            if (openFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            file = openFileDialog1.FileName;
        }
        switch (t)
        {
		case ParameterType.Display:
			displayParameters = JsonUtility.FromJson<DisplayParameters> (File.ReadAllText (file));
			//TODO Eventual multidisplay implemntation
			/*
			if (id_displays != null) {
				List<TODisplay> tmp_list = new List<TODisplay> ();
				for (int i = 0; i < id_displays.Length; i++)
					tmp_list.Add (displayParameters.displays.Find (x => x.name == "Display " + id_displays [i]));
				displayParameters.displays = tmp_list;
			}*/
			TOLaunchParameters.filePathDisplay = file;
                break;
		case ParameterType.Nodes: nodesParameters = JsonUtility.FromJson<TONodeParameters>(File.ReadAllText(file));TOLaunchParameters.filePathNodes = file; break;
		case ParameterType.Tracker: trackerParameters = JsonUtility.FromJson<TrackerParameters>(File.ReadAllText(file));TOLaunchParameters.filePathTracker = file; break;
		case ParameterType.Launch: TOLaunchParameters.launchSettings = JsonUtility.FromJson<TOLaunchList>(File.ReadAllText(file)); break;
        } 
    }


	/// <summary>
	/// Saves a config file. if no file is specified, a window dialog is open
	/// </summary>
	/// <param name="t">Type</param>
	/// <param name="file">Fil.</param>
	public static void SaveTOParameters(ParameterType t,string file=null)
	{
		if (file == null) {
			SaveFileDialog saveFileDialog = new SaveFileDialog ();

			saveFileDialog.Filter = string.Format ("{0} configuration file|*.{0}.json", t);
			saveFileDialog.Title = string.Format ("Choose {0} configuration file", t);
			if (saveFileDialog.ShowDialog () != System.Windows.Forms.DialogResult.OK)
				return;

			file = saveFileDialog.FileName;
		}

		string tmp = "";
		switch (t) {
			case ParameterType.Display:
				tmp = JsonUtility.ToJson (displayParameters, true);
				break;
			case ParameterType.Nodes:
				tmp = JsonUtility.ToJson (nodesParameters, true);
				break;
			case ParameterType.Tracker:
				tmp = JsonUtility.ToJson (trackerParameters, true);
				break;
		case ParameterType.Launch:
			tmp = JsonUtility.ToJson (TOLaunchParameters.launchSettings, true);
			break;
		}
		File.WriteAllText (file, tmp);
    }

}



