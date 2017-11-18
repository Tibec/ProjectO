using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPosition : MonoBehaviour {
	//Script pour la position du menu par rapport au joueur, il ne devra pas bouger, et rester à distance fixe.
	public GameObject PlayerHead;
	public Vector2 Offset;
	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
        float r = PlayerHead.transform.rotation.eulerAngles.y * Mathf.PI / 180.0f;
        float x, y, z;

        x = PlayerHead.transform.position.x + (Offset.x * Mathf.Sin(r));
        y = PlayerHead.transform.position.y + Offset.y;
        z = PlayerHead.transform.position.z + (Offset.x * Mathf.Cos(r));

        transform.position = new Vector3(x, y, z);
        transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x, 
            PlayerHead.transform.rotation.eulerAngles.y, 
            0);

    }
}
