using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionListenerData {
    public GameObject sender { get; set; }
    public Collider collision { get; set; }
    public Vector3 contactPoint;
}

public class CollisionListener : MonoBehaviour
{

    public GameObject listener;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        // since ontriggerenter does not give the contact point
        // we need to compute it ...
        Vector3 direction = (other.transform.position - transform.position);
        Vector3 contactPoint = transform.position + direction;

        listener.SendMessage("OnRemoteCollisionEnter", 
            new CollisionListenerData
            {
                sender = gameObject,
                collision = other,
                contactPoint = contactPoint

            });
    }

    private void OnCollisionEnter(Collision other)
    {
        listener.SendMessage("OnRemoteCollisionEnter", 
            new CollisionListenerData
            {
                sender = gameObject,
                collision = other.collider,
                contactPoint = other.contacts[0].point
            });
    }
    private void OnTriggerExit(Collider other)
    {
        listener.SendMessage("OnRemoteCollisionExit", new CollisionListenerData { sender = gameObject, collision = other });
    }

    private void OnCollisionExit(Collision other)
    {
        listener.SendMessage("OnRemoteCollisionExit", new CollisionListenerData { sender = gameObject, collision = other.collider });
    }
}
