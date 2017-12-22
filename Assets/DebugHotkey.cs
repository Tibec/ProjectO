using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHotkey : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if(Input.GetKeyDown(KeyCode.P))
        {
            FindObjectOfType<HudManager>().AddMenu(FindObjectOfType<OpenMenu>().menu);
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            Vector3 p = FindObjectOfType<TOCAVEController>().transform.position;
            p.y = 0.8f;
            FindObjectOfType<TOCAVEController>().transform.position = p;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 p = FindObjectOfType<TOCAVEController>().transform.position;
            float angle = FindObjectOfType<TOCAVEController>().transform.eulerAngles.y * Mathf.PI / 180f;

            p.x += 0.03f * Mathf.Sin(angle);
            p.z += 0.03f * Mathf.Cos(angle);

            FindObjectOfType<TOCAVEController>().transform.position = p;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 p = FindObjectOfType<TOCAVEController>().transform.position;
            float angle = FindObjectOfType<TOCAVEController>().transform.eulerAngles.y * Mathf.PI / 180f;
            p.x -= 0.03f * Mathf.Sin(angle);
            p.z -= 0.03f * Mathf.Cos(angle);
            FindObjectOfType<TOCAVEController>().transform.position = p;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 p = FindObjectOfType<TOCAVEController>().transform.eulerAngles;
            p.y -= 1;
            FindObjectOfType<TOCAVEController>().transform.eulerAngles = p;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 p = FindObjectOfType<TOCAVEController>().transform.eulerAngles;
            p.y += 1;
            FindObjectOfType<TOCAVEController>().transform.eulerAngles = p;
        }

        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            FindObjectOfType<HudManager>().CloseMenu(0);
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            FindObjectOfType<HudManager>().CloseMenu(1);
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            FindObjectOfType<HudManager>().CloseMenu(2);
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            FindObjectOfType<HudManager>().CloseMenu(3);
        }

        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            FindObjectOfType<HudManager>().CloseMenu(4);
        }

        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            FindObjectOfType<HudManager>().rotateLeft = true;
        }
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            FindObjectOfType<HudManager>().rotateRight = true;
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            //FindObjectOfType<HudManager>().CloseMenu(4);
        }
    }
}
