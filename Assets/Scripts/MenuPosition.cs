using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPosition : MonoBehaviour {
	//Script pour la position du menu par rapport au joueur, il ne devra pas bouger, et rester à distance fixe.
	public GameObject PlayerHead;
	public Vector2 Offset;

    public float distanceBeforeMovement = 0.5f;
    public float angleBeforeRotation = 60f;

    private bool moving = false;
    private bool rotating = false;
    // Use this for initialization
    void Start () {


	}

	public void Teleport()
	{
		transform.position = ComputeDestination ();
		transform.rotation = Quaternion.Euler(
			
			new Vector3(
				transform.rotation.eulerAngles.x,
				PlayerHead.transform.rotation.eulerAngles.y,
				0)
		);
	}

	private Vector3 ComputeDestination()
	{
		float r = PlayerHead.transform.rotation.eulerAngles.y * Mathf.PI / 180.0f;
		Vector3 newPosition = new Vector3();
	
		newPosition.x = PlayerHead.transform.position.x + (Offset.x * Mathf.Sin(r));
		newPosition.y = PlayerHead.transform.position.y + Offset.y;
		newPosition.z = PlayerHead.transform.position.z + (Offset.x * Mathf.Cos(r));
	
		return newPosition;
	}

	// Update is called once per frame
	void Update () {
         
		Vector3 newPosition = ComputeDestination ();
        /*
        // should we move ?
        if (Vector3.Distance(transform.position, newPosition) > distanceBeforeMovement && !moving)
            moving = true;

        // should we rotate ?
        if (Mathf.DeltaAngle(PlayerHead.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.y) > angleBeforeRotation && !rotating)
            rotating = true;


        */
        if (Vector3.Distance(transform.position, newPosition) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, newPosition, 0.03f);
        }/*
        else if (moving)
            moving = false;

        if (rotating && Mathf.DeltaAngle(PlayerHead.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.y) > 0.01f)
        {
        */
        transform.rotation = Quaternion.Euler(
                new Vector3(
                    transform.rotation.eulerAngles.x,
                    PlayerHead.transform.rotation.eulerAngles.y,
                    0)
            );
        /*
        }
        else if (rotating)
            rotating = false;
        */
    }
}
