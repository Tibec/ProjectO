using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TOInteractiveItem))]
public class TOGrabItem : MonoBehaviour {



	private Vector3 oldPosition;
	private Quaternion oldRotation;
	private GameObject oldParent = null;
	private TOInteractiveItem m_InteractiveItem;
	private bool grabbed=false;

	private void Awake ()
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

		
	}


		//Handle the Out event
	private void HandleOut(TORaycaster raycaster)
	{

		
	}


		//Handle the Click event
	private void HandleClick(TORaycaster raycaster)
	{
		if (raycaster.CurrentInteractible != null) {
			if (!grabbed) {

				oldPosition = transform.position;
				oldRotation = transform.rotation;
                if (transform.parent != null)
                    oldParent = transform.parent.gameObject;
                else
                    oldParent = null;

				transform.parent = raycaster.parent.transform;

				grabbed = true;
			} else {

                if (oldParent != null)
                    transform.parent = oldParent.transform;
                else
                    transform.parent = null;

				transform.position = oldPosition;
				transform.rotation = oldRotation;

				grabbed = false;
			}
		}

		
	}


		//Handle the DoubleClick event
	private void HandleDoubleClick(TORaycaster raycaster)
	{

		
	}



}
