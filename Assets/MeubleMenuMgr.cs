using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeubleMenuMgr : MonoBehaviour {

    public int activePanel;
    public List<GameObject> panels;

    public Transform assignedFurniture;

    //scaling purpose
    private GameObject targetContainer;
    // movement
    private GameObject target;


    // Use this for initialization
    void Start () {
        targetContainer = assignedFurniture.Find("Object").gameObject;
        target = targetContainer.transform.GetChild(0).gameObject;
        activePanel = GetTargetState() ? 1 : 0 ; 
    }

    // Update is called once per frame
    void Update () {
        for (int i = 0; i < panels.Count; ++i)
        {
            panels[i].SetActive(i == activePanel);
        }
	}

    public void SetTargetState(bool _free)
    {
        target.GetComponent<Rigidbody>().isKinematic = !_free;
        target.GetComponent<Rigidbody>().useGravity = _free;
        foreach (Collider c in target.GetComponents<Collider>()) c.isTrigger = !_free;

        //free = _free;
    }

    // return true if free
    public bool GetTargetState()
    {
        return !target.GetComponent<Rigidbody>().isKinematic;
    }

    public void OnClick(MenuButton btn)
    {
        if (btn.name == "MoveBtn")
        {
            SetTargetState(true);
            activePanel = 0;
        }
        else if (btn.name == "ResizeBtn")
        {
            activePanel = 2;
        }
        else if (btn.name == "TrashBtn")
        {
            assignedFurniture.GetComponent<MeubleInteraction>().menuOpen = false;
            Destroy(assignedFurniture.gameObject);
        }
        else if (btn.name == "LockBtn")
        {
            SetTargetState(false);
            activePanel = 0;
        }
        else if (btn.name == "PlusBtn")
        {
            targetContainer.transform.localScale += Vector3.one * 0.1f;
        }
        else if (btn.name == "MinusBtn")
        {
            targetContainer.transform.localScale -= Vector3.one * 0.1f;
        }
    }

}
