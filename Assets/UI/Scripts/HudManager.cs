using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HudSettings))]
public class HudManager : MonoBehaviour {

    public delegate void PopupCallback(bool ok);

    private HudSettings settings;

    public List<GameObject> OpenedMenu = new List<GameObject>();

    public GameObject RotationBar;

    private int activeWindow;

    private bool handleNewMenu;


    public Transform MenusContainer;
    public GameObject ToBeOpened;
    public bool openingInProgress;

    private float currentMenuRadius;
    private float currentMenuHeight;

    public bool rotateLeft, rotateRight;

    // Use this for initialization
    void Start () {

        if (MenusContainer == null)
            Debug.LogError("MenuContainer not assigned !");

        DontDestroyOnLoad(this);

        settings = GetComponent<HudSettings>();

        OpenedMenu.Clear();

        RotationBar.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (rotateLeft) { Rotate(true); rotateLeft = false; }
        if (rotateRight) { Rotate(true); rotateRight = false; }


        // Update HUD position
        if (settings.TargetToFollow != null)
        {
            transform.position = settings.TargetToFollow.position;
            MenusContainer.position = settings.TargetToFollow.position + Vector3.up * settings.HudHeight;
            if (OpenedMenu.Count == 1)
            {
                transform.rotation = settings.TargetToFollow.rotation;
            }
            else if (OpenedMenu.Count > 1)
            {
                RotationBar.transform.localPosition = ComputeRotationBarPosition();
                RotationBar.transform.localRotation = ComputeRotationBarRotation();
            }
        }

        // update window distance if settings have been changed
        if( currentMenuRadius != settings.MenuRadius  )
        {
            currentMenuRadius = settings.MenuRadius;
            for(int i = 0;i<OpenedMenu.Count;++i)
            {
                OpenedMenu[i].transform.localPosition = ComputeMenuPosition(i);
            }
        }

        // Spawn menu if needed
        if (ToBeOpened != null && !openingInProgress)
        {
            openingInProgress = true;

            ToBeOpened = Instantiate(ToBeOpened, transform);
            ToBeOpened.SetActive(true);
            OpenedMenu.Add(ToBeOpened);
            ToBeOpened.transform.localPosition = ComputeMenuPosition(0);
            ToBeOpened.transform.localRotation = Quaternion.Euler(settings.MenuInclinaison, 0, 0);
            Action after = () =>
            {
                ToBeOpened.transform.parent = MenusContainer;
                ToBeOpened = null;
                openingInProgress = false;
                if (OpenedMenu.Count >= 2)
                    RotationBar.SetActive(true);
                activeWindow = OpenedMenu.Count - 1; 
            };

            if (OpenedMenu.Count == 1)
                after();
            else
                StartCoroutine(CoroutineWithCallback(RotateLeft(), after));
        }

    }


    #region Public API 
    public void AddMenu(GameObject prefab)
    {
        if (OpenedMenu.Count == settings.MaxMenuWindow)
        {
            print("Tried to open a menu, but too many already opened");
            return;
        }

        if (ToBeOpened != null)
        {
            print("Tried to open a menu, but an other one is still being opened : " + ToBeOpened.name);
            return;
        }

        ToBeOpened = prefab;
    
    }

    public void CloseMenu(int id)
    {

    }

    public bool PopupConfirm(string content, bool warning, PopupCallback callback)
    {
        return true;
    }

    public void PopupInfo(string content)
    {

    }
    #endregion

    #region Internal Utilities

    private Vector3 ComputeRotationBarPosition()
    {
        Vector3 v = new Vector3();
        float angle = settings.TargetToFollow.rotation.eulerAngles.y * Mathf.PI / 180.0f;
        v.x = settings.MenuRadius * Mathf.Sin(angle);
        v.y = settings.RotationHeight;
        v.z = settings.MenuRadius * Mathf.Cos(angle);
        return v;
    }

    private Quaternion ComputeRotationBarRotation()
    {
        return Quaternion.Euler(0, settings.TargetToFollow.rotation.eulerAngles.y, 0);
    }

    private Vector3 ComputeMenuPosition(int count)
    {
        Vector3 v = new Vector3();
        float angle = 360f / settings.MaxMenuWindow * count % 180f;
        angle = angle * Mathf.PI / 180.0f;
        v.x = settings.MenuRadius * Mathf.Sin(angle);
        v.z = settings.MenuRadius * Mathf.Cos(angle);
        return v;
    }

    private IEnumerator RotateTo(float angle)
    {
        yield break;
    }

    private IEnumerator RotateLeft()
    {
        float origin = MenusContainer.localEulerAngles.y;
        if(Mathf.Round(origin) % (360f / settings.MaxMenuWindow) != 0)
        {
            print("a rotation is already running");
            yield break;
        }

        float goal = origin - (360f / settings.MaxMenuWindow);
        while (Mathf.Abs(Mathf.DeltaAngle(MenusContainer.localEulerAngles.y,  goal)) > 1f)
        {
            MenusContainer.localEulerAngles = new Vector3(0, Mathf.LerpAngle(MenusContainer.localEulerAngles.y, goal, 0.1f), 0);
            yield return new WaitForEndOfFrame();
        }
        MenusContainer.localEulerAngles = new Vector3(0, goal, 0);
    }

    private IEnumerator RotateRight()
    {
        float origin = MenusContainer.localEulerAngles.y;
        if (Mathf.Round(origin) % (360f / settings.MaxMenuWindow) != 0)
        {
            print("a rotation is already running");
            yield break;
        }
        float goal = origin + (360f / settings.MaxMenuWindow);
        while (Mathf.Abs(Mathf.DeltaAngle(MenusContainer.localEulerAngles.y, goal)) > 1f)
        {
            transform.localEulerAngles = new Vector3(0, Mathf.LerpAngle(transform.localEulerAngles.y, goal, 0.1f), 0);
            yield return new WaitForEndOfFrame();
        }
        MenusContainer.localEulerAngles = new Vector3(0, goal, 0);
    }

    IEnumerator CoroutineWithCallback(IEnumerator doFirst, Action doLast)
    {
        yield return doFirst;
        doLast();
    }

    #endregion
    public void Rotate(float angle)
    {

    }

    // rotate to left if true, else right
    public void Rotate(bool left)
    {
        if (left)
        {
            StartCoroutine(CoroutineWithCallback(RotateLeft(), () => {
                --activeWindow;
            }));
        }
        else
        {
            StartCoroutine(CoroutineWithCallback(RotateRight(), () => {
                ++activeWindow;
            }));
        }
    }
}
