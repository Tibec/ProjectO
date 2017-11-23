using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPosition : MonoBehaviour {
	//Script pour la position du menu par rapport au joueur, il ne devra pas bouger, et rester à distance fixe.
	public GameObject PlayerHead;
	public Vector2 Offset;

    public float delayBeforeMovement = 30;
    public float distanceBeforeMovement = 0.5f;
    private bool timerActive = false;
    private float timer = 0;
    private bool canMove = false;
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

        // should we move ?
        if (Vector3.Distance(transform.position, newPosition) > distanceBeforeMovement && !timerActive)
        {
            // Start the timer
            timer = delayBeforeMovement;
            timerActive = true;
        }


        if (timer < 0 && timerActive)
        {
            canMove = true;
            timerActive = false;
        }
        else
            --timer;

        if (canMove && Vector3.Distance(transform.position, newPosition) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, newPosition, 0.03f);
            transform.rotation = Quaternion.Euler(/*Vector3.Lerp(
                transform.rotation.eulerAngles,*/
                new Vector3(
                    transform.rotation.eulerAngles.x,
                    PlayerHead.transform.rotation.eulerAngles.y,
                    0)/*,
                0.05f
                )*/
            );
        }
        else if (canMove)
            canMove = false;
    }
}
