using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPosition : MonoBehaviour {
	//Script pour la position du menu par rapport au joueur, il ne devra pas bouger, et rester à distance fixe.
	public GameObject player;
	private Vector3 offset_menu;
	// Use this for initialization
	void Start () {
		offset_menu = transform.position - player.transform.position;

	}
	
	// Update is called once per frame
	void Update () {
		transform.position = offset_menu + player.transform.position;
	}
}
