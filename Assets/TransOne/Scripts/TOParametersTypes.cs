using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
/// <summary>
/// General Types
/// </summary>
public enum ParameterType
{
	Display,
	Nodes,
	Tracker,
	Launch
}

//===========================================
//					DISPLAY
//===========================================


[Serializable]
/// <summary>
/// Sets the positions and resolutions of the applications
/// </summary>
public struct DisplayParameters
{

	public List<TODisplay> displays;
	/// <summary>
	/// Invert the eyes
	/// </summary>
	public bool stereoInvert;

}


[Serializable]
/// <summary>
/// Represents a display (hardware)
/// </summary>
public class TODisplay{
	public string name;
	public Vector2 resolution;
	public List<TOScreen> screens;
	public bool fullscreen;

	public TODisplay(){
		name = "Display";
		resolution = Vector2.zero;
		screens = new List<TOScreen> ();
		fullscreen = true;
	}
	public TODisplay(string s)
	{
		name = s;
		resolution = Vector2.zero;
		screens = new List<TOScreen>();
		fullscreen = true;
	}
}


[Serializable]
/// <summary>
/// Represents a point of view in the application
/// </summary>
public class TOScreen{
	public string name;
	public Rect r;
	/// <summary>
	/// Id of plane/camera node we use
	/// </summary>
	public int id_node;

	public TOScreen(){
		name = "Screen";
		r = new Rect();
		id_node = -1;
	}
	public TOScreen(string s)
	{
		name = s;
		r = new Rect();
		id_node = -1;
	}
}




//===========================================
//					NODES
//===========================================

//S
[Serializable]
/// <summary>
/// Sets the position of the planes and cameras
//Note : use node tree for this class
/// </summary>
public struct TONodeParameters
{
	//Root node
	public TONode root;
	public List<TOPlane> planes;


}

//A
[Serializable]
/// <summary>
/// A simple Node
/// note : Only represent the root node for now, should be use for a node tree
/// </summary>
public class TONode{
	public string name;
	/// <summary>
	/// Position
	/// </summary>
	public Vector3 pos;
	/// <summary>
	/// Rotation
	/// </summary>
	public Vector3 rot;
	/// <summary>
	/// Parent node (not in use for now)
	/// </summary>
	public TONode parent;

	public TONode()
	{
		name = "Node";
		pos = new Vector3();
		rot = new Vector3();
		parent = null;
	}
	public TONode(string s)
	{
		name = s;
		pos = new Vector3();
		rot = new Vector3();
		parent = null;

	}


}
//T
[Serializable]
/// <summary>
///Type of a node 
///Plane : Face of a Cave, will generate automaticaly a stereo camera
///Camera : a simple camera
/// </summary>
public enum TONodeType
{
	Plane,
	Camera
}

//

[Serializable]
/// <summary>
/// A plane node. Note: Is used for now as a subnode, should be a TONode child class and we should make a TOCamera child class too
/// </summary>
public class TOPlane{
	public string name;
	public Vector3 pos;
	public Vector3 rot;
	public Vector2 size;
	public TONodeType type;
	public int id;

	public TOPlane()
	{
		name = "Plane";
		pos = new Vector3();
		rot = new Vector3();
		size = new Vector2();
		type = TONodeType.Plane;
		id = -1;
	}
	public TOPlane(string s)
	{
		name = s;
		pos = new Vector3();
		rot = new Vector3();
		size = new Vector2();
		type = TONodeType.Plane;
		id = -1;
	}
	public TOPlane(int i)
	{
		name = "Plane " + i;
		pos = new Vector3();
		rot = new Vector3();
		size = new Vector2();
		type = TONodeType.Plane;
		id = i;
	}

}



//===========================================
//					TRACKER
//===========================================

//


[Serializable]
/// <summary>
/// Sets the parameters for all trackers. Note : We have to make a struct to serialize the object
/// </summary>
public struct TrackerParameters
{
	public List<TOTrackerServer> trackerServers;

}
	
[Serializable]
/// <summary>
///Parameters of a tracker server
/// </summary>
public class TOTrackerServer
{
	public string name;
	public TOTrackerServerType type;
	public string ip; //ip of the server
	public int port;
	public List<TOTracker> trackers; //trackers association

	public TOTrackerServer()
	{
		name = "trackerServer";
		type = TOTrackerServerType.NoServer;
		ip = "localhost";
		port = 0;
		trackers = new List<TOTracker>();
	}

	public TOTrackerServer(TOTrackerServerType t)
	{

		name = "trackerServer";
		type =t;
		ip = "localhost";
		port = 0;
		trackers = new List<TOTracker>();
	}


}


//Type of the tracker server
//NoServer means we use the current PC drivers
[Serializable]
public enum TOTrackerServerType
{
	NoServer,
	VRPN,
    Vive,
	//Optitrack,
}

//Parameters of a tracker
[Serializable]
public class TOTracker
{
	public int id; //Tracker ID
	public string name;
	public TOTrackerType type;

	public TOTracker()
	{
		id = 0;
		name = "tracker";
		type = TOTrackerType.Mouse;
	}

	public TOTracker(int i)
	{
		id = i;
		name = "Tracker " +i;
		type = TOTrackerType.Mouse;
	}
}



[Serializable]
/// <summary>
/// The type of a tracker (only mouse,keyboard,navctrl work for now) 
/// </summary>
public enum TOTrackerType
{
	Mouse =0,
	Keyboard= 1,
	NavCtrl =2,
    Vive =3,
    GenericVRPN=4,
	Wiimote =5,
	Xbox =6
    //TODO
	//RazerHydra =6,
	//OptitrackRB =7,
	//OptitrackSK= 8
}


//===========================================
//					LAUNCH
//===========================================


[Serializable]
/// <summary>
/// Sets the launch parameters for the launcher application
///Note : if no launch settings are set, we launch the application normally
/// </summary>
public static class TOLaunchParameters
{
	public static TOLaunchList launchSettings;
	public static string filePathExe ="",filePathDisplay="",filePathNodes="",filePathTracker="";
}

//We have to make a struct to serialize the object
[Serializable]
public struct TOLaunchList
{
	public List<TOLaunch> launchlist;

}



[Serializable]
/// <summary>
/// Parameters of a node application
/// </summary>
public class TOLaunch
{
	public string name;
	public TOLaunchType type;
	public string ip;
	public int port;
	/// <summary>
	/// list of display to use. It only use one display for now
	/// </summary>
	public List<int> displays_id; //

	public TOLaunch()
	{
		name = "App";
		type = TOLaunchType.Server;
		ip = "localhost";
		port = 0;
		displays_id = new List<int> ();

	}

	public TOLaunch(TOLaunchType t)
	{

		name = "trackerServer";
		type =t;
		ip = "localhost";
		port = 0;
		displays_id = new List<int> ();
	}

}

[Serializable]
public enum TOLaunchType
{
	Server,
	Client,
}

