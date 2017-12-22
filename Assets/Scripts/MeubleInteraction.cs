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
    private float handDist;
    private bool followHand = false;
    private Transform parentBackup;
    private Transform hand1;
    private Transform hand2;

    private FurnitureSettings settings;

    // Use this for initialization
    void Start () {
        settings = FindObjectOfType<FurnitureSettings>();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(followHand)
        {
            
            if(Mathf.Abs(Vector3.Distance(hand1.transform.position, hand2.transform.position) - handDist) > settings.maxHandDiffToMove)
            {
                followHand = false;
                GetComponentInChildren<Rigidbody>().isKinematic = false;
                transform.parent = parentBackup;
                hand1 = hand2 = null;
                return;
            }
        }

    }

    private void StartFollowHand()
    {
        followHand = true;
        handDist = Vector3.Distance(hands[0].transform.position, hands[1].transform.position);
        parentBackup = transform.parent;
        hand1 = hands[0].transform;
        hand2 = hands[1].transform;
        transform.parent = hands[0].transform;
        GetComponentInChildren<Rigidbody>().isKinematic = true;
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.tag != "Player")
            return;

        hands.Add(c);

        if (!menuOpen) 
        {
            StartCoroutine(CheckHoldHand(c));
        }

        if(hands.Count == 2 && free && !followHand)
        {
            if (hands[0] == hands[1])
                return; // ?? Black magic
            StartCoroutine(CheckTryMove());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnTriggerEnter(collision.collider);
    }

    private IEnumerator CheckHoldHand(Collider c)
    {
        float timePassed = 0f;
        while (timePassed < settings.secondsToOpenMenu)
        {
            yield return new WaitForFixedUpdate();
            timePassed += Time.fixedDeltaTime;
            if (!hands.Contains(c) || hands.Count == 2)
            {
                ApplyOpenMenuMaterial(-1);
                yield break;
            }
            ApplyOpenMenuMaterial(timePassed / settings.secondsToOpenMenu);
        }
        OpenMenu();
        ApplyOpenMenuMaterial(-1);
    }

    private IEnumerator CheckTryMove()
    {
        float timePassed = 0f;
        while (timePassed < settings.secondsToOpenMenu)
        {
            yield return new WaitForFixedUpdate();
            timePassed += Time.fixedDeltaTime;
            if ( hands.Count != 2)
            {
                ApplyColor(Color.yellow , -1);
                yield break;
            }
            ApplyColor(Color.yellow, timePassed / settings.secondsToOpenMenu);
        }
        ApplyColor(Color.yellow, -1);
        StartFollowHand();
    }


    // if opacity = -1 => remove material
    private void ApplyOpenMenuMaterial(float opacity)
    {
        MeshRenderer[] r = GetComponentsInChildren<MeshRenderer>();
        foreach (var mesh in r)
        {
            foreach (var material in mesh.materials)
            {
                material.EnableKeyword("_EMISSION");
                if (opacity == -1)
                    material.SetColor("_EmissionColor", Color.black);
                else
                    material.SetColor("_EmissionColor", new Color(opacity, opacity, opacity));

            }
        }
    }

    private void ApplyColor(Color c, float intensity)
    {
        MeshRenderer[] r = GetComponentsInChildren<MeshRenderer>();
        foreach (var mesh in r)
        {
            foreach (var material in mesh.materials)
            {
                material.EnableKeyword("_EMISSION");
                if (intensity == -1)
                    material.SetColor("_EmissionColor", Color.black);
                else
                    material.SetColor("_EmissionColor", Color.Lerp(Color.black, c, intensity));

            }
        }
    }

    public void HighlightSelection(bool enable)
    {
        MeshRenderer[] r = GetComponentsInChildren<MeshRenderer>();
        foreach (var mesh in r)
        {
            foreach (var material in mesh.materials)
            {
                material.EnableKeyword("_EMISSION");
                if (!enable)
                    material.SetColor("_EmissionColor", Color.black);
                else
                    material.SetColor("_EmissionColor", new Color(1, 0, 0, 0.5f));

            }
        }
    }

    void OnTriggerExit(Collider data)
    {
        if (data.tag != "Player")
            return;
        hands.Remove(data);
    }

    private void OnCollisionExit(Collision collision)
    {
        OnTriggerExit(collision.collider);
    }

    void OpenMenu()
    {
        if (menuOpen)
            return;
        HudManager hud = FindObjectOfType<HudManager>();
        meubleMenuPrefab.GetComponent<MeubleMenuMgr>().assignedFurniture = transform;
        hud.AddMenu(meubleMenuPrefab);

        menuOpen = true;

    }
}
