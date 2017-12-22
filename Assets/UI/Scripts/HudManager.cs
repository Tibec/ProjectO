using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(HudSettings))]
public class HudManager : MonoBehaviour {

    private HudSettings settings;

    public List<GameObject> OpenedMenu = new List<GameObject>();

    private bool handleNewMenu;
    private bool recomputeMenu;


    public GameObject ToBeOpened;
    public bool openingInProgress;

    private float currentMenuRadius;
    private float currentMenuHeight;

    public bool rotateLeft, rotateRight;
    public float rotateDestination;
    public bool isRotating;

    public delegate void PopupCallback(bool ok);

    private List<Transform> spawnPoints = new List<Transform>();

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
        // DontDestroyOnLoad(this);

        settings = GetComponent<HudSettings>();

        if (settings.MenusContainer == null)
            Debug.LogError("MenuContainer not assigned !");

        OpenedMenu.Clear();

        settings.RotationBar.SetActive(false);

        isRotating = false;

        ComputeSpawnPoints();
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
        if( currentMenuRadius != settings.MenuRadius )
        {
            currentMenuRadius = settings.MenuRadius;
            ReComputeSpawnPoints();
        }
        if (recomputeMenu )
        {
            for(int i = 0;i<spawnPoints.Count;++i)
            {
                if (spawnPoints[i].childCount == 0)
                {
                    for (int j = i+1; j < spawnPoints.Count; ++j)
                    {
                        if (spawnPoints[j].childCount != 0)
                        {
                            Transform t = spawnPoints[i];
                            spawnPoints[i] = spawnPoints[j];
                            spawnPoints[j] = t;
                        }
                    }
                }
            }
            StartCoroutine(RecomputeSpawnPoints());
            recomputeMenu = false;
        }

        // Spawn menu if needed
        if (ToBeOpened != null && !openingInProgress)
        {
            openingInProgress = true;

            AudioSource audio = GetComponent<AudioSource>();
            if (audio != null)
                audio.Play();

            ToBeOpened = Instantiate(ToBeOpened, spawnPoints[OpenedMenu.Count]);
            ToBeOpened.SetActive(true);
            OpenedMenu.Add(ToBeOpened);
            ToBeOpened.transform.localRotation = Quaternion.Euler(settings.MenuInclinaison, 0, 0);
            Action after = () =>
            {
                ToBeOpened = null;
                openingInProgress = false;
                if (OpenedMenu.Count >= 1)
                {
                    settings.RotationBar.SetActive(true);
                    settings.RotationBar.transform.localPosition = Vector3.zero;
                }
            };

            if (OpenedMenu.Count == 1)
            {
                settings.MenusContainer.transform.eulerAngles = settings.TargetToFollow.eulerAngles.y * Vector3.up;
                after();
            }
            else
            {
                RotateToMenu(OpenedMenu.Count-1);
                after();
            }
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
                if (OpenedMenu.IndexOf(item) == OpenedMenu.Count - 1) { Rotate(false); }
                ret =  true;
            }
            else
                ret = false;
            ++index;
            return ret;
        });
        if (deletedCount > 0)
            recomputeMenu = true;
        if(OpenedMenu.Count == 0)
        {
            settings.RotationBar.gameObject.SetActive(false);
        }

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
            PopupInfo("Erreur", "Trops de menu sont déjà ouverts ! Veuillez en fermer quelques un !");
            print("Tried to open a menu, but too many already opened");
            return;
        }

        ToBeOpened = prefab;
    
    }

    public void CloseMenu(int id)
    {
        Destroy(OpenedMenu[id]);
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

    private void ComputeSpawnPoints()
    {
        int slot = settings.MaxMenuWindow;
        for (int i = 0; i < slot; ++i)
        {
            Vector3 pos = ComputeMenuPosition(i);
            float angle = i * (360f / slot);
            GameObject go = new GameObject(i.ToString());
            go.transform.parent = settings.MenusContainer.transform;
            go.transform.localPosition = pos;
            go.transform.localEulerAngles = angle * Vector3.up;
            spawnPoints.Add(go.transform);
        }
    }

    private void ReComputeSpawnPoints()
    {
        int slot = settings.MaxMenuWindow;
        for (int i = 0; i < slot; ++i)
        {
            Vector3 pos = ComputeMenuPosition(i);
            float angle = i * (360f / slot);
            spawnPoints[i].gameObject.name = i.ToString();
            spawnPoints[i].localPosition = pos;
            spawnPoints[i].localEulerAngles = angle * Vector3.up;
        }
    }

    IEnumerator RecomputeSpawnPoints()
    {
        int slot = settings.MaxMenuWindow;

        for (int j = 0; j < 120; ++j)
        {
            for (int i = 0; i < slot; ++i)
            {
                Vector3 pos = ComputeMenuPosition(i);
                float angle = i * (360f / slot);
                spawnPoints[i].localPosition = Vector3.Lerp(pos, spawnPoints[i].localPosition, 0.1f);
                spawnPoints[i].localEulerAngles = angle * Vector3.up;
            }

            yield return new WaitForEndOfFrame();
        }
        ReComputeSpawnPoints();
    }



    private Vector3 ComputeMenuPosition(int count)
    {
        Vector3 v = new Vector3();
        float angle = 360f / settings.MaxMenuWindow * count;
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

    private IEnumerator RotateTo(float goal)
    {
        if(isRotating)
        {
            print("a rotation is already running");
            yield break;
        }

        isRotating = true;
        rotateDestination = goal;
        while (Mathf.Abs(Mathf.DeltaAngle(settings.MenusContainer.localEulerAngles.y, rotateDestination)) > 1f)
        {
            settings.MenusContainer.localEulerAngles = new Vector3(0, Mathf.LerpAngle(settings.MenusContainer.localEulerAngles.y, rotateDestination, 0.1f), 0);
            yield return new WaitForEndOfFrame();
        }
        settings.MenusContainer.localEulerAngles = new Vector3(0, rotateDestination, 0);

        isRotating = false;
    }


    IEnumerator CoroutineWithCallback(IEnumerator doFirst, Action doLast)
    {
        yield return doFirst;
        doLast();
    }

    #endregion
    public void Rotate(float angle)
    {
        throw new NotImplementedException();
    }

    private void RotateToMenu(int v)
    {
        float goal = 0f;
        float menuAngle = 360f / settings.MaxMenuWindow;
        float playerAngle = settings.TargetToFollow.eulerAngles.y;
        goal = playerAngle - v % OpenedMenu.Count * menuAngle;
        if (!isRotating)
            StartCoroutine(RotateTo(goal));
        else
            rotateDestination = goal;
    }

    // rotate to left if true, else right
    public void Rotate(bool left)
    {
        float goal = 0f;
        float menuAngle = 360f / settings.MaxMenuWindow;
        float playerAngle = settings.TargetToFollow.eulerAngles.y;
        if (GetComponent<HudActiveWindow>().activeWindow == -1)
        {
            if (left)
                goal = playerAngle;
            else
                goal = playerAngle - (OpenedMenu.Count - 1) * menuAngle;
        }
        else
        {
            if (left)
                goal = playerAngle - (GetComponent<HudActiveWindow>().activeWindow - 1) % OpenedMenu.Count * menuAngle;
            else
                goal = playerAngle - (GetComponent<HudActiveWindow>().activeWindow + 1) % OpenedMenu.Count * menuAngle;
        }

        if (!isRotating)
            StartCoroutine(RotateTo(goal));
        else
            rotateDestination = goal;
    }
}
