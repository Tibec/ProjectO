using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(HudSettings))]
public class HudManager : MonoBehaviour {

    private HudSettings settings;

    public List<GameObject> OpenedMenu = new List<GameObject>();


    private int activeWindow;

    private bool handleNewMenu;
    private bool recomputeMenu;


    public GameObject ToBeOpened;
    public bool openingInProgress;

    private float currentMenuRadius;
    private float currentMenuHeight;

    public bool rotateLeft, rotateRight;

    public bool isMoving;

    public delegate void PopupCallback(bool ok);

    [Serializable]
    public class PopupData
    {
        public string title;
        public string content;
        public bool isQuestion;
        public PopupCallback callback;
    }

    public List<PopupData> PopupQueue;


    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);

        settings = GetComponent<HudSettings>();

        if (settings.MenusContainer == null)
            Debug.LogError("MenuContainer not assigned !");

        OpenedMenu.Clear();

        settings.RotationBar.SetActive(false);

        activeWindow = -1;

        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (rotateLeft) { Rotate(true); rotateLeft = false; }
        if (rotateRight) { Rotate(false); rotateRight = false; }


        // Update Popup position
        if (settings.TargetToFollow != null)
        {
            settings.Popup.localPosition = ComputePopupPosition();
            settings.Popup.localRotation = ComputeRotationBarRotation();
        }

        // update window distance if settings have been changed
        if( currentMenuRadius != settings.MenuRadius || recomputeMenu )
        {
            currentMenuRadius = settings.MenuRadius;
            for(int i = 0;i<OpenedMenu.Count;++i)
            {
                OpenedMenu[i].transform.localPosition = ComputeMenuPosition(i);
            }
            recomputeMenu = false;
        }

        // Spawn menu if needed
        if (ToBeOpened != null && !openingInProgress)
        {
            openingInProgress = true;

            AudioSource audio = GetComponent<AudioSource>();
            if (audio != null)
                audio.Play();

            ToBeOpened = Instantiate(ToBeOpened, transform);
            ToBeOpened.SetActive(true);
            OpenedMenu.Add(ToBeOpened);
            ToBeOpened.transform.localPosition = ComputeMenuPosition(0);
            ToBeOpened.transform.localRotation = Quaternion.Euler(settings.MenuInclinaison, 0, 0);
            Action after = () =>
            {
                ToBeOpened.transform.parent = settings.MenusContainer;
                ToBeOpened = null;
                openingInProgress = false;
                if (OpenedMenu.Count >= 2)
                {
                    settings.RotationBar.SetActive(true);
                    settings.RotationBar.transform.localPosition = Vector3.zero;
                }
                activeWindow = OpenedMenu.Count - 1; 
            };

            if (OpenedMenu.Count == 1)
                after();
            else
                StartCoroutine(CoroutineWithCallback(RotateLeft(), after));
        }

        // spawn popup if needed
        if(!settings.Popup.gameObject.activeSelf && PopupQueue.Count > 0)
        {
            settings.Popup.gameObject.SetActive(true);

            PopupData d = PopupQueue[0];
            settings.Popup.GetComponent<PopupContent>().SetContent(d.title, d.content, d.isQuestion, d.callback);
            PopupQueue.RemoveAt(0);
        }

        // delete menu if there is a null ptr
        int index = 0;
        int deletedCount = OpenedMenu.RemoveAll(item => 
        {
            bool ret;
            if (item == null)
            {
                if (index == activeWindow) { StartCoroutine(RotateRight()); }
                ret =  true;
            }
            else
                ret = false;
            ++index;
            return ret;
        });
        if (deletedCount > 0)
            recomputeMenu = true;

    }


    #region Public API 
    public void AddMenu(GameObject prefab)
    {
        if (ToBeOpened != null)
        {
            print("Tried to open a menu, but an other one is still being opened : " + ToBeOpened.name);
            return;
        }

        if (OpenedMenu.Count == settings.MaxMenuWindow)
        {
            print("Tried to open a menu, but too many already opened");
            return;
        }

        ToBeOpened = prefab;
    
    }

    public void CloseMenu(int id)
    {

    }

    public void PopupConfirm(string title, string content, bool warning, PopupCallback callback)
    {
        PopupData p = new PopupData
        {
            title = title,
            content = content,
            isQuestion = true,
            callback = callback
        };
        PopupQueue.Add(p);
    }

    public void PopupInfo(string title, string content)
    {
        PopupData p = new PopupData
        {
            title = title,
            content = content,
            isQuestion = false,
        };
        PopupQueue.Add(p);
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
        return Quaternion.Euler(0, settings.TargetToFollow.localRotation.eulerAngles.y, 0);
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

    private Vector3 ComputePopupPosition()
    {
        Vector3 v = new Vector3();
        float angle = settings.TargetToFollow.rotation.eulerAngles.y * Mathf.PI / 180.0f;
        v.x = settings.PopupRadius * Mathf.Sin(angle);
        v.z = settings.PopupRadius * Mathf.Cos(angle);
        return v;
    }

    private IEnumerator RotateTo(float angle)
    {
        yield break;
    }

    private IEnumerator RotateLeft()
    {
        activeWindow = (activeWindow + 1) % settings.MaxMenuWindow;
        float origin = settings.MenusContainer.localEulerAngles.y;
        if(Mathf.Round(origin) % (360f / settings.MaxMenuWindow) != 0)
        {
            print("a rotation is already running");
            yield break;
        }

        float goal = origin - (360f / settings.MaxMenuWindow);
        while (Mathf.Abs(Mathf.DeltaAngle(settings.MenusContainer.localEulerAngles.y,  goal)) > 1f)
        {
            settings.MenusContainer.localEulerAngles = new Vector3(0, Mathf.LerpAngle(settings.MenusContainer.localEulerAngles.y, goal, 0.1f), 0);
            yield return new WaitForEndOfFrame();
        }
        settings.MenusContainer.localEulerAngles = new Vector3(0, goal, 0);
    }

    private IEnumerator RotateRight()
    {
        activeWindow = (activeWindow + settings.MaxMenuWindow - 1) % settings.MaxMenuWindow;
        float origin = settings.MenusContainer.localEulerAngles.y;
        if (Mathf.Round(origin) % (360f / settings.MaxMenuWindow) != 0)
        {
            print("a rotation is already running");
            yield break;
        }
        float goal = origin + (360f / settings.MaxMenuWindow);
        while (Mathf.Abs(Mathf.DeltaAngle(settings.MenusContainer.localEulerAngles.y, goal)) > 1f)
        {
            settings.MenusContainer.localEulerAngles = new Vector3(0, Mathf.LerpAngle(settings.MenusContainer.localEulerAngles.y, goal, 0.1f), 0);
            yield return new WaitForEndOfFrame();
        }
        settings.MenusContainer.localEulerAngles = new Vector3(0, goal, 0);
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
