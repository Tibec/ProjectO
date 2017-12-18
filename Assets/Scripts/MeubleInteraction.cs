using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeubleInteraction : MonoBehaviour {

    public float minScale;
    public float maxScale;

    public GameObject meubleMenuPrefab;

	public bool menuOpen = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnRemoteCollisionEnter(CollisionListenerData data)
    {
        if (!menuOpen && data.collision.tag == "Player") 
        {
            HudManager hud = FindObjectOfType<HudManager>();
            meubleMenuPrefab.GetComponent<MeubleMenuMgr>().assignedFurniture = transform;
            hud.AddMenu(meubleMenuPrefab);

            menuOpen = true;

            /*
            GameObject menu = Instantiate(meubleMenuPrefab, transform);
            //menu.transform = data.

            Vector3 spawn = data.contactPoint;
            Transform playerHead = FindObjectOfType<TOCAVEController>().transform;
            spawn = Vector3.MoveTowards(spawn, playerHead.position, 0.1f);

            menu.GetComponent<MeubleMenuPosition>().attachPoint = data.contactPoint - data.sender.transform.position;
            menu.GetComponent<MeubleMenuPosition>().attachedObject = data.sender.transform;
            */
        }
    }

    void OnRemoteCollisionExit(CollisionListenerData data)
    {

    }

}
