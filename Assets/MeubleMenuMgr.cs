using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeubleMenuMgr : MonoBehaviour {

    public int activePanel;
    public List<GameObject> panels;

    public Transform assignedFurniture;


    // Use this for initialization
    void Start () {
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
        assignedFurniture.GetComponent<Rigidbody>().isKinematic = !_free;
        assignedFurniture.GetComponent<Rigidbody>().useGravity = _free;
        foreach (Collider c in assignedFurniture.GetComponentsInChildren<Collider>())
            c.isTrigger = !_free;

        assignedFurniture.GetComponent<MeubleInteraction>().free = _free;
    }

    // return true if free
    public bool GetTargetState()
    {
        return !assignedFurniture.GetComponent<Rigidbody>().isKinematic;
    }

    public void OnClick(MenuButton btn)
    {
        if (btn.name == "MoveBtn")
        {
            SetTargetState(true);
            activePanel = 1;
        }
        else if (btn.name == "ResizeBtn")
        {
            activePanel = 2;
        }
        else if (btn.name == "BackBtn")
        {
            activePanel = 0;
        }
        else if (btn.name == "TrashBtn")
        {
            assignedFurniture.GetComponent<MeubleInteraction>().menuOpen = false;
            Destroy(assignedFurniture.gameObject);
            Destroy(gameObject);
        }
        else if (btn.name == "LockBtn")
        {
            SetTargetState(false);
            activePanel = 0;
        }
        else if (btn.name == "PlusBtn")
        {
            assignedFurniture.transform.localScale = 
                Vector3.Min(assignedFurniture.transform.localScale + Vector3.one * 0.1f, 
                assignedFurniture.GetComponent<MeubleInteraction>().maxScale * Vector3.one
                );
        }
        else if (btn.name == "MinusBtn")
        {
            assignedFurniture.transform.localScale =
                Vector3.Max(assignedFurniture.transform.localScale - Vector3.one * 0.1f,
                assignedFurniture.GetComponent<MeubleInteraction>().minScale * Vector3.one
                );
        }
    }

    private void OnDestroy()
    {
        assignedFurniture.GetComponent<MeubleInteraction>().menuOpen = false;
    }

}
