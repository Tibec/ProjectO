using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Meuble : MonoBehaviour {

    private bool free;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetState(bool _free)
    {
        GetComponent<Rigidbody>().isKinematic = !_free;
        GetComponent<Rigidbody>().useGravity = _free;
        foreach (Collider c in GetComponents<Collider>()) c.isTrigger = !_free;

        free = _free;
    }

    public void SetState(GameObject _prefab)
    {
        
    }


    // void OnTouch
}
