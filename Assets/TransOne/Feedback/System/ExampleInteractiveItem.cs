using UnityEngine;


// This script is a simple example of how an interactive item can
// be used to change things on gameobjects by handling events.
[RequireComponent(typeof(TOInteractiveItem))]
public class ExampleInteractiveItem : MonoBehaviour
{
    
    private TOInteractiveItem m_InteractiveItem;


    private void Awake()
    {

        m_InteractiveItem = GetComponent<TOInteractiveItem>();
       

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

        print("On Over");
    }

    //Handle the Out event
    private void HandleOut(TORaycaster raycaster)
    {
        print("On Out");

    }


    //Handle the Click event
    private void HandleClick(TORaycaster raycaster)
    {
        print("On click");

    }


    //Handle the DoubleClick event
    private void HandleDoubleClick(TORaycaster raycaster)
    {
        print("On Double click");

    }
}