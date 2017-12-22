using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TOInteractiveItem))]
public class Highlight : MonoBehaviour {

    private Material material;
    [SerializeField]
    private Color OverColor = Color.blue;
    [SerializeField]
    private Color ClickedColor= Color.red;
    [SerializeField]
    private Color DoubleClickedColor = Color.green;
    private TOInteractiveItem m_InteractiveItem;
    private Renderer m_Renderer;


    private void Awake()
    {
        m_Renderer = GetComponent<Renderer>();
        m_InteractiveItem = GetComponent<TOInteractiveItem>();
        material = new Material(m_Renderer.material);

    }


    private void OnEnable()
    {
        m_InteractiveItem.OnOver += HandleOver;
        m_InteractiveItem.OnOut += HandleOut;
        m_InteractiveItem.OnDown += HandleClick;
        m_InteractiveItem.OnDoubleInput += HandleDoubleClick;
    }


    private void OnDisable()
    {
        m_InteractiveItem.OnOver -= HandleOver;
        m_InteractiveItem.OnOut -= HandleOut;
        m_InteractiveItem.OnDown -= HandleClick;
        m_InteractiveItem.OnDoubleInput -= HandleDoubleClick;
    }


    //Handle the Over event
    private void HandleOver(TORaycaster raycaster)
    {

        m_Renderer.material.color = OverColor;
    }


    //Handle the Out event
    private void HandleOut(TORaycaster raycaster)
    {

        m_Renderer.material = material;
    }


    //Handle the Click event
    private void HandleClick(TORaycaster raycaster)
    {

        m_Renderer.material.color = ClickedColor;
    }


    //Handle the DoubleClick event
    private void HandleDoubleClick(TORaycaster raycaster)
    {

        m_Renderer.material.color = DoubleClickedColor;
    }
}
