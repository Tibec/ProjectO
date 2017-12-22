using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum RaycastMode
{
    Line,
    Sphere
}

// In order to interact with objects in the scene
// this class casts a ray into the scene and if it finds
// a VRInteractiveItem it exposes it for other classes to use.
// This script should be generally be placed on the camera.
public class TORaycaster : MonoBehaviour
{

    public event Action<RaycastHit> OnRaycasthit;
    public event Action<Collider> OnColliderHit;

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

    // This event is called every frame that the user's gaze is over a collider.
    [SerializeField]
    private LayerMask m_ExclusionLayers;           // Layers to exclude from the raycast.
    //[SerializeField]
    //private Reticle m_Reticle;                     // The reticle, if applicable.
    [SerializeField]
    private string nameInput;                     // Used to call input based events on the current TOInteractiveItem.
    [SerializeField]
    private RaycastMode raycastMode;                     // Raycast Mode : Shoot a line or a sphere
    [SerializeField]
    private bool m_ShowRay;                   // Optionally show the  ray.
    [SerializeField]
    private float m_RayLength = 1f;              // How far into the scene the ray is cast.

    public GameObject[] shapePrefabs;
    public Material shapeMaterial;
    private GameObject objShape;
    private Vector3 objSize;

    private TOInteractiveItem m_CurrentInteractible;                //The current interactive item
    private TOInteractiveItem m_LastInteractible;                   //The last interactive item
    private float hidist = 0.0f;

    Collider[] c;
    // Utility for other classes to get the current interactive item
    public TOInteractiveItem CurrentInteractible
    {
        get { return m_CurrentInteractible; }
    }


    public  void InitMetaphor()
    {
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

        UpdateShape();
        objSize = objShape.transform.localScale;
        UpdateColor();

    }



    void Start()
    {
        TOInteractionInput i = TOInputController.GetInstance.interactionInputs.Find(x => nameInput == x.nameInput);
        i.OnInput += HandleClick;
        i.OnDoubleInput += HandleDoubleClick;
        i.OnInputUp += HandleUp;
        i.OnInputDown += HandleDown;
        InitMetaphor();


    }


    void OnDestroy()
    {
        TOInteractionInput i = TOInputController.GetInstance.interactionInputs.Find(x => nameInput == x.nameInput);
        i.OnInput -= HandleClick;
        i.OnDoubleInput -= HandleDoubleClick;
        i.OnInputUp -= HandleUp;
        i.OnInputDown -= HandleDown;
    }


    private void Update()
    {


            
        UpdateColor();
        UpdateSize();
        UpdateRaycast();
        
     
      

    }

    public void UpdateSize()
    {

        if (raycastMode == RaycastMode.Line)
            objShape.transform.localScale = new Vector3(objSize.x, objSize.y, m_RayLength);
        else
            objShape.transform.localScale = new Vector3(m_RayLength, m_RayLength, m_RayLength);
    }


    public void UpdateColor()
    {
        objShape.GetComponentInChildren<MeshRenderer>().material.color = color;
    }
    public void UpdateShape()
    {
        if (objShape != null) Destroy(objShape);
        objShape = Instantiate<GameObject>(shapePrefabs[(int)raycastMode], refnode);
        objShape.name = "Shape";
        objShape.GetComponentInChildren<MeshRenderer>().material = shapeMaterial;
    }



    private void UpdateRaycast()
    {
        // Show the debug ray if required

        // Create a ray that points forwards from the camera.
        Ray ray = new Ray(refnode.position, refnode.forward);
        RaycastHit hit;

        // Do the raycast forweards to see if we hit an interactive item
        if (raycastMode == RaycastMode.Line)
        {
            if (Physics.Raycast(ray, out hit, m_RayLength, ~m_ExclusionLayers))
            {
                TOInteractiveItem interactible = hit.collider.GetComponent<TOInteractiveItem>(); //attempt to get the VRInteractiveItem on the hit object
                m_CurrentInteractible = interactible;
                hidist = hit.distance;
                // If we hit an interactive item and it's not the same as the last interactive item, then call Over
                if (interactible && interactible != m_LastInteractible)
                    interactible.Over(this);

                // Deactive the last interactive item 
                if (interactible != m_LastInteractible)
                    DeactiveLastInteractible();

                m_LastInteractible = interactible;

                // Something was hit, set at the hit position.
                //if (m_Reticle)
                //    m_Reticle.SetPosition(hit);

                if (OnRaycasthit != null)
                    OnRaycasthit(hit);
            }
            else
            {
                // Nothing was hit, deactive the last interactive item.
                DeactiveLastInteractible();
                m_CurrentInteractible = null;

                // Position the reticle at default distance.
                //if (m_Reticle)
                //    m_Reticle.SetPosition();
            }



        }



        else
        {
            c = Physics.OverlapSphere(refnode.position, m_RayLength, ~m_ExclusionLayers);

            if (c.Length != 0)
            {
                for (int i = 0; i < c.Length; i++)
                {
                    TOInteractiveItem interactible = c[i].GetComponent<TOInteractiveItem>(); //attempt to get the VRInteractiveItem on the hit object
                    if (interactible != null)
                    {
                        m_CurrentInteractible = interactible;
                        // If we hit an interactive item and it's not the same as the last interactive item, then call Over
                        if (interactible && interactible != m_LastInteractible)
                            interactible.Over(this);

                        // Deactive the last interactive item 
                        if (interactible != m_LastInteractible)
                            DeactiveLastInteractible();

                        m_LastInteractible = interactible;

                        // Something was hit, set at the hit position.
                        //if (m_Reticle)
                        //    m_Reticle.SetPosition(hit);

                        if (OnColliderHit != null)
                            OnColliderHit(c[0]);

                        return;
                    }

                }


                // No interactable object was hit, deactive the last interactive item.
                DeactiveLastInteractible();
                m_CurrentInteractible = null;

                // Position the reticle at default distance.
                //if (m_Reticle)
                //    m_Reticle.SetPosition();


            }
            else
            {
                // Nothing was hit, deactive the last interactive item.
                DeactiveLastInteractible();
                m_CurrentInteractible = null;

                // Position the reticle at default distance.
                //if (m_Reticle)
                //    m_Reticle.SetPosition();
            }


        }

    }




    private void DeactiveLastInteractible()
    {
        if (m_LastInteractible == null)
            return;

        m_LastInteractible.Out(this);
        m_LastInteractible = null;
    }


    private void HandleUp()
    {
        if (m_CurrentInteractible != null)
            m_CurrentInteractible.Up(this);
    }


    private void HandleDown()
    {
        if (m_CurrentInteractible != null)
            m_CurrentInteractible.Down(this);
    }


    private void HandleClick()
    {
        if (m_CurrentInteractible != null)
            m_CurrentInteractible.Click(this);
    }


    private void HandleDoubleClick()
    {
        if (m_CurrentInteractible != null)
            m_CurrentInteractible.DoubleClick(this);

    }
}

