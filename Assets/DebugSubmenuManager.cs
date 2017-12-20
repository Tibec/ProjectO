using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[Serializable]
public class DebugSetting
{
    public string name;
    public MonoBehaviour component;
    [Tooltip("Must be int or float")]
    public string fieldName;
    public Type fieldType;
    public FieldInfo fieldInfo;
}

public class DebugSubmenuManager : MonoBehaviour {

    public List<DebugSetting> Fields;

    public Transform KeysContainer;

    public DebugKey IntKeyPrefab;
    public DebugKey FloatKeyPrefab;

    public int ItemsPerLine;
    public int ItemsPerColumn;
    public int CanvaHeight = 800;
    public int CanvaWidth = 1400;

    private int spawnedItems = 0;
    
    // Use this for initialization
    void Start () {
        InitializeKeys();
    }

    private void InitializeKeys()
    {
        foreach (var s in Fields)
        {
            FieldInfo fi = s.component.GetType().GetField(s.fieldName);
            if (fi == null)
            {
                Debug.LogError(string.Format("Cannot found a field named '{0}' in component '{1}' type of gameobject '{2}'",
                    s.fieldName, s.component.GetType(), s.component.name));
                continue;
            }
            if (fi.FieldType == typeof(float))
            {
                s.fieldType = typeof(float);
                s.fieldInfo = fi;
                DebugKey k = Instantiate(FloatKeyPrefab, KeysContainer);
                k.linkedSetting = s;
                k.transform.localPosition = ComputeKeyPosition();
                ++spawnedItems;
            }
            else if (fi.FieldType == typeof(int))
            {
                s.fieldType = typeof(int);
                s.fieldInfo = fi;
                DebugKey k = Instantiate(IntKeyPrefab, KeysContainer);
                k.linkedSetting = s;
                k.transform.localPosition = ComputeKeyPosition(); 
                ++spawnedItems;
            }
            else
            {
                Debug.LogError(string.Format("Cannot add the field named '{0}' in component '{1}' type of gameobject '{2}' to the debug menu because the type : {3} is not handled yet !",
                    s.fieldName, s.component.GetType(), s.component.name));
                continue;
            }
        }
    }

    private Vector3 ComputeKeyPosition()
    {
        Vector3 v = new Vector3();
        v.x = (spawnedItems / ItemsPerColumn ) * (CanvaWidth / ItemsPerLine);
        v.y = (spawnedItems % ItemsPerColumn) * -(CanvaHeight / ItemsPerColumn);
        return v;
    }



    // Update is called once per frame
    void Update () {
		
	}
}
