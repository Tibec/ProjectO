using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //transform.localPosition = TOGenericVRPN.GetPosition(0);
        /*
		if (TOWiimote.GetButton (WiiInputs.A,1))
            print("ok");
		
		print (TOWiimote.GetAxis (WiiInputs.DPad, 1));*/
        /*
		if (PSNavCtrl.GetButtonDown (PSInputs.Cross, 0))
			print ("ok");*/
        /*
                if (TOViveCtrl.GetButtonDown(ValveInputs.PadTouch, 1))
                    print("ok");*/

        if (TOInput.GetButtonDown(KeyCode.Mouse0, 0))
            print("ok1");

    }

    
}
