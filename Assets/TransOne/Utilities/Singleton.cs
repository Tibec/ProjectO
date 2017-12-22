using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour  where T : Object{

    public static T instance;

	public static bool CheckSingletonExists(T m) {

		if (instance !=null) 
		{
			Debug.LogError ("There must be only one " + m.GetType().ToString());
			Destroy (m);
			return true;
		}
        else
        {
            instance = m;
            return false;
        } 
		


    }

}
