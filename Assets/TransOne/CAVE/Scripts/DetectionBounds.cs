using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary> Show bounds for a CAVE plane </summary>
public class DetectionBounds : MonoBehaviour {

    
    Material bounds;
    Color c;
    
  
    void Start () 
	{

        
        bounds = GetComponent<MeshRenderer>().material;
        GetComponent<MeshRenderer>().material.mainTextureScale= new Vector2(5 * transform.localScale.x, 5 * transform.localScale.y);
        c = GetComponent<MeshRenderer>().material.GetColor("_TintColor");
    }

	void Update () 
	{
        float dist = float.PositiveInfinity;
		//TODO we should only use the second case
        if (TOInputController.GetInstance.positionTrackers.Count == 0 )
        {
            Vector3 pos = TOCAVEController.GetInstance.transform.localPosition;
            dist = Vector3.Dot(pos - transform.localPosition, Quaternion.Inverse(transform.parent.rotation) * (-1.0f * transform.forward));
        }
        else
        {
            for (int i = 0; i < TOInputController.GetInstance.positionTrackers.Count; i++)
            {
                Vector3 pos = TOInputController.GetInstance.positionTrackers[i].localPosition;
                float dist2 = Vector3.Dot(pos - transform.localPosition, Quaternion.Inverse(transform.parent.rotation) * (-1.0f * transform.forward));
                if (dist2 < dist)
                    dist = dist2;
            }
        }

		c.a = Mathf.InverseLerp(TOCAVEController.GetInstance.boundsDetectionMin, 0.0f, dist);
        bounds.SetColor("_TintColor", c);

	}



}

