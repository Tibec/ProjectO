using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum FeedbackType
{
    EBAGG,Arrow,Trace,
    Highlight
}

/// <summary>
/// base class to create metaphor for transone
/// </summary>
public abstract class TOMetaphor : MonoBehaviour {

   
    [NonSerialized]
    public FeedbackType type;
    [NonSerialized]
    public Transform refnode;


    /// <summary>
    /// Where the metaphor is
    /// if null, parent is where the script is placed
    /// </summary>
    public GameObject parent;
    /// <summary>
    /// Color of the metaphor
    /// </summary>
    public Color color = Color.black;
    /// <summary>
    /// Set the metaphor visible or not
    /// </summary>
    public bool visible = true;
    /// <summary>
    /// Position offset
    /// </summary>
    public Vector3 posOffset = Vector3.zero;
    /// <summary>
    /// Rotation offset
    /// </summary>
    public Vector3 rotOffset = Vector3.zero;
    public Vector3 size = Vector3.one;
   



    public virtual void InitMetaphor() {
        GameObject g = new GameObject("Metaphor");
        if (parent == null)
        {
            parent = this.gameObject;
        }

        g.transform.position = parent.transform.position;
        g.transform.rotation = parent.transform.rotation;
        g.transform.localScale = parent.transform.localScale;
        g.transform.parent = parent.transform;
        refnode = g.transform;

        


    }

  

    
    public virtual void UpdateSize() {
        refnode.localScale = size;
    }
    public virtual void UpdateColor()
    {
        //Depends on the metaphor
    }
    public virtual void UpdatePosition() {
        refnode.localPosition = posOffset;  

    }
    public virtual void UpdateRotation() {
        refnode.localEulerAngles = Vector3.zero;
        refnode.RotateAround(parent.transform.position, parent.transform.forward, rotOffset.z);
        refnode.RotateAround(parent.transform.position, parent.transform.up, rotOffset.y);
        refnode.RotateAround(parent.transform.position, parent.transform.right, rotOffset.x);
    }

    public virtual void UpdateRotationLocal()
    {
        refnode.localEulerAngles = rotOffset;
        

    }


    public virtual void UpdateVisible()
    {
        refnode.gameObject.SetActive(visible);
    }
    


}
