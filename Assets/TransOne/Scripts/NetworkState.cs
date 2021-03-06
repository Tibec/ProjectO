using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;



/// <summary>
/// Manage the data exchanges between the applications
/// </summary>
public class NetworkState : NetworkManager {


	void Start()
	{
		this.networkAddress = TOParameters.ip_server;
		this.networkPort = TOParameters.port;

		if (TOParameters.isServer )
			this.StartServer ();
		else if (TOParameters.isClient)
			this.StartClient ();
	}
		


	public override void OnClientConnect (NetworkConnection conn)
	{
		base.OnClientConnect (conn);
		Debug.Log("Connected to server and ready");
	
	}

	public override void OnStartServer ()
	{
		base.OnStartServer ();
		Debug.Log( "Server initialized." );

	}


	public override void OnServerConnect (NetworkConnection conn)
	{
		base.OnServerConnect (conn);
	}


	public override void OnServerReady (NetworkConnection conn)
	{
		base.OnServerReady (conn);
		NetworkServer.SpawnObjects ();
	}

	/*
	void OnServerInitialized()
	{
		Debug.Log( "Server initialized and ready." );
		NetworkServer.SpawnObjects ();
        Debug.Log("ok");
	}

	void OnServerReady(NetworkConnection conn){

		Debug.Log ("ok");
		//NetworkServer.SpawnObjects ();
	}

	void OnServerConnected(NetworkConnection conn) {
		Debug.Log( "Node connected " + conn.address );
		client_count++;
	}

	
	void OnServerDisconnect( NetworkConnection node ) {
		Debug.Log( "Clean up after player " + node );
		//NetworkServer.DestroyPlayersForConnection (node);
	}
	
	public virtual void OnClientConnect()
	{
		Debug.Log( "Connected to server." );
	}
	
	void OnFailedToConnect(NetworkConnectionError error)
	{
		Debug.Log( "Could not connect to server: " + error );
	}

*/

	void OnDestroy() {
		
	if (TOParameters.isServer)
		{
			Network.Disconnect();
		}
	else if (TOParameters.isClient)
		{
			Network.CloseConnection( Network.player, true );                
		}
	}
	
}
