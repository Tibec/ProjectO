using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeubleInteraction : MonoBehaviour {

    public float minScale;
    public float maxScale;

    public bool free = true;

    public GameObject meubleMenuPrefab;

	public bool menuOpen = false;
    private List<Collider> hands = new List<Collider>();
    private Vector3 handOffset;

    private FurnitureSettings settings;

    // Use this for initialization
    void Start () {
        settings = FindObjectOfType<FurnitureSettings>();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(hands.Count == 2 && free)
        {
            float yDiff = Mathf.Abs(hands[0].transform.position.y - hands[1].transform.position.y);
            if(yDiff < settings.maxHandDiffToMove)
            {
                transform.position = hands[1].transform.position + handOffset;
            }
        }
	}

    void OnRemoteCollisionEnter(CollisionListenerData data)
    {
        if (!menuOpen && data.collision.tag == "Player") 
        {
            StartCoroutine(WaitOneSec());
        }
        if(data.collision.tag == "Player")
        {
            handOffset = transform.position - data.collision.transform.position;
            hands.Add(data.collision);
        }
    }

    private IEnumerator WaitOneSec()
    {
        yield return new WaitForSeconds(settings.secondsToOpenMenu);
        OpenMenu();
    }

    void OnRemoteCollisionExit(CollisionListenerData data)
    {
        hands.Remove(data.collision);
    }

    void OpenMenu()
    {
        HudManager hud = FindObjectOfType<HudManager>();
        meubleMenuPrefab.GetComponent<MeubleMenuMgr>().assignedFurniture = transform;
        hud.AddMenu(meubleMenuPrefab);

        menuOpen = true;

    }
}
