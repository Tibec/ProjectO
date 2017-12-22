using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class to enable network features on transone metaphors
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class TOMetaphorNetwork<T> where T:TOMetaphor {
    
    public int id;
    /// <summary>
    /// The name set in redis
    /// </summary>
    public string nameObject = "";
    /// <summary>
    /// update rate in seconds
    /// </summary>
    public float rate = 0.5f;
    /// <summary>
    /// (For redis only) Activate data broadcast to the redis server
    /// </summary>
    public bool server;
    /// <summary>
    /// (For redis only) Activate data reception from the redis server
    /// </summary>
    public bool client;
    /// <summary>
    /// (For redis only) Don't delete the database at Application Quit
    /// </summary>
    public bool persistentData;
    /// <summary>
    /// Activate metaphor update with ANGORA data
    /// </summary>
    public bool ANGORA=false;
    /// <summary>
    /// Session id we want to use
    /// </summary>
    public int ANGORASessionID=0;
    /// <summary>
    /// The reference gesture we want to use
    /// </summary>
    public int ANGORARefGesID = 0;
    /// <summary>
    /// changes the reference point of the received data from ANGORA  (by default it is Unity's world space) 
    ///  </summary>
    public Transform root = null;
    /// <summary>
    /// Should be always false
    /// </summary>
    public bool legacyRoot = false;
    /// <summary>
    /// Update the metaphor according to the implementation
    /// </summary>
    /// <param name="metaphor"></param>
    public abstract void UpdateANGORA(T metaphor);

}
