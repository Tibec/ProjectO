using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FloatDebugKey : DebugKey {

    public Text textVal;
    public Text textName;

    private bool shouldInc;
    private bool shouldDec;
    private float currentValue;

    public float incrementStep = 1f;
    public List<MenuButton> scaleButtons;

    bool firstUpdate = true;

    // Use this for initialization
    void Start()
    {
        textName.text = linkedSetting.name;
        currentValue = GetValue();
        textVal.text = currentValue.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(firstUpdate)
        {
            firstUpdate = false;
            ChangeActiveBtn(scaleButtons[0]);
        }

        if (shouldInc)
        {
            shouldInc = false;
            SetValue(currentValue + incrementStep);
        }
        else if (shouldDec)
        {
            shouldDec = false;
            SetValue(currentValue - incrementStep);
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
        else if (btn.name == "UnitScaleButton")
        {
            ChangeActiveBtn(btn);
            incrementStep = 1f;
        }
        else if (btn.name == "TenthScaleButton")
        {
            ChangeActiveBtn(btn);
            incrementStep = 0.1f;
        }
        else if (btn.name == "HundrethButton")
        {
            ChangeActiveBtn(btn);
            incrementStep = 0.01f;
        }
        else if (btn.name == "ThousandthButton")
        {
            ChangeActiveBtn(btn);
            incrementStep = 0.001f;
        }

    }

    private void ChangeActiveBtn(MenuButton newActiveBtn)
    {
        foreach(var b in scaleButtons)
        {
            if (b == newActiveBtn)
                b.SetSelected(true);
            else
                b.SetSelected(false);
        }
    }

    private float GetValue()
    {
        return (float)linkedSetting.fieldInfo.GetValue(linkedSetting.component);
    }

    private void SetValue(float val)
    {
        linkedSetting.fieldInfo.SetValue(linkedSetting.component, val);
    }
}
