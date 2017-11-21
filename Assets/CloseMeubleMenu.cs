using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseMeubleMenu : MonoBehaviour {

    // Use this for initialization
    void Start () {



    }

    // Update is called once per frame
    void Update () {

    }

    void GestureDetected(string emitterName)
    {
        if (emitterName == "closeMenu")
        {
            Destroy(this.gameObject);
        }
    }

}
