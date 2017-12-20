using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntDebugKey : DebugKey {

    public Text textVal;
    public Text textName;

    private bool shouldInc;
    private bool shouldDec;
    private int currentValue;



	// Use this for initialization
	void Start ()
    {
        textName.text = linkedSetting.name;
        currentValue = GetValue();
        textVal.text = currentValue.ToString();
    }

    // Update is called once per frame
    void Update ()
    {
        if (shouldInc)
        {
            shouldInc = false;
            SetValue(currentValue + 1);
        }
        else if (shouldDec)
        {
            shouldDec = false;
            SetValue(currentValue - 1);
        }

        if (GetValue() != currentValue)
        {
            currentValue = GetValue();
            textVal.text = currentValue.ToString();
        }

    }

    void OnClick(MenuButton btn)
    {
        if (btn.name == "PlusButton")
        {
            shouldInc = true;
        }
        else if (btn.name == "MinusButton")
        {
            shouldDec = true;
        }
    }

    private int GetValue()
    {
        return (int)linkedSetting.fieldInfo.GetValue(linkedSetting.component);
    }

    private void SetValue(int val)
    {
        linkedSetting.fieldInfo.SetValue(linkedSetting.component, val);
    }
}
