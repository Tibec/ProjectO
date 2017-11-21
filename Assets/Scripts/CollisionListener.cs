using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class CollisionListenerData { public GameObject sender { get; set; } public Collider collision { get; set; } }

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
        listener.SendMessage("OnRemoteCollisionEnter", new CollisionListenerData { sender = gameObject, collision = other });
    }

    private void OnCollisionEnter(Collision other)
    {
        listener.SendMessage("OnRemoteCollisionEnter", new CollisionListenerData { sender = gameObject, collision = other.collider });
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
