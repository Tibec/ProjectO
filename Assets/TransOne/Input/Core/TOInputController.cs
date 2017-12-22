using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The controller class for TransOne Input
/// </summary>
public class TOInputController : MonoBehaviour {


    public static TOInputController GetInstance
    {
        get
        {
            return Singleton<TOInputController>.instance;
        }
        
    }
    /// <summary>
    /// list of all devices in the scene
    /// </summary>
    static List<BasicInputTO> listInputs = new List<BasicInputTO>();

    [NonSerialized]
    public List<Transform> positionTrackers = new List<Transform>();

    public List<TOInteractionInput> interactionInputs = new List<TOInteractionInput>();
    

    void Awake()
    {
        if(Singleton<TOInputController>.CheckSingletonExists(this)) return;


        if (TOParameters.trackerParameters.trackerServers.Count==0)
		{
			this.enabled = false;
		}
        else
        {
            TOParameters.InitTrackers();
            for (int i = 0; i < interactionInputs.Count; i++)
            {
                interactionInputs[i].SetType();
            }
        }
    }


    public static void UpdateData(int id)
    {
		listInputs.AddRange(TOInput.returnInstance(id));

    }

    void Update()
    {
        for(int i =0;i< interactionInputs.Count; i++)
        {
            interactionInputs[i].CheckInput();
        }


    }

    void LateUpdate()
    {
        for(int i=0; i < listInputs.Count; i++)
        {
            listInputs[i].ev.Invoke();
        }


    }

	void OnDestroy(){
		TOInput.instances = new Dictionary<int, TOInput>();
	}

}




