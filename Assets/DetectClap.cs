using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DetectClap : MonoBehaviour {

    float lastClap = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != "Player")
            return;

        if(Time.realtimeSinceStartup - lastClap <= 1.5f )
        {
            TutoScript ts = FindObjectOfType<TutoScript>();
            if (ts != null)
            {
                Destroy(ts.gameObject);
                FindObjectOfType<BubbleMgr>().FadeOut();
                lastClap = 0;
                return;
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else
        {
            lastClap = Time.realtimeSinceStartup;
        }
    }
}
