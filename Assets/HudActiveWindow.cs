using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudActiveWindow : MonoBehaviour {

    private HudSettings settings;
    private HudManager manager;

    public int activeWindow;

    // Use this for initialization
    void Start () {
        settings = GetComponent<HudSettings>();
        manager = GetComponent<HudManager>();

    }

    // Update is called once per frame
    void Update () {

        if (manager.OpenedMenu.Count == 0)
        {
            activeWindow = -1;
            return;
        }

        // Compute the active menu
        float bestDelta = Mathf.Abs(Mathf.DeltaAngle(manager.OpenedMenu[0].transform.eulerAngles.y,
            settings.TargetToFollow.eulerAngles.y));
        int bestDeltaIndex = 0;
        for(int i = 1; i < manager.OpenedMenu.Count; ++i)
        {
            if(Mathf.Abs(Mathf.DeltaAngle(manager.OpenedMenu[i].transform.eulerAngles.y,
                settings.TargetToFollow.eulerAngles.y)) < bestDelta)
            {
                bestDeltaIndex = i;
                bestDelta = Mathf.Abs(Mathf.DeltaAngle(manager.OpenedMenu[i].transform.eulerAngles.y,
                    settings.TargetToFollow.eulerAngles.y));
            }
        }
        if (bestDelta > 360f / settings.MaxMenuWindow / 2)
            activeWindow = -1;
        else
            activeWindow = bestDeltaIndex;
        
        // Update menu status
        UpdateWindowStatus();

    }

    void UpdateWindowStatus()
    {
        for(int i=0;i< manager.OpenedMenu.Count;++i)
        {
            if (i == activeWindow)
            {
                manager.OpenedMenu[i].GetComponentInChildren<MenuData>().Enabled = true;
                manager.OpenedMenu[i].GetComponentInChildren<CanvasGroup>().alpha = 1;
            }
            else
            {
                manager.OpenedMenu[i].GetComponentInChildren<MenuData>().Enabled = false;
                manager.OpenedMenu[i].GetComponentInChildren<CanvasGroup>().alpha = 0.2f;
            }
        }
    }
}
