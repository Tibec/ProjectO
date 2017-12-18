using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupContent : MonoBehaviour
{

    public GameObject buttonAccept;
    public GameObject buttonRefuse;
    public GameObject buttonDismiss;

    public Text title;
    public Text content;

    private HudManager.PopupCallback callback;

    public AudioClip infoSound;
    public AudioClip questionSound;

    public float hideSpeed = 0.05f;

    private AudioSource audioPlayer;

    // Use this for initialization
    void Awake()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetContent(string title, string content, bool isQuestion, HudManager.PopupCallback callback = null)
    {
        this.title.text = title;
        this.content.text = content;

        if (isQuestion)
        {
            buttonDismiss.SetActive(false);
            buttonAccept.SetActive(true);
            buttonRefuse.SetActive(true);
            this.callback = callback;
            audioPlayer.clip = questionSound;
        }
        else
        {
            buttonDismiss.SetActive(true);
            buttonAccept.SetActive(false);
            buttonRefuse.SetActive(false);
            this.callback = null;
            audioPlayer.clip = infoSound;
        }

        audioPlayer.Play();
    }

    public void OnClick(MenuButton btn)
    {
        if (btn.gameObject == buttonDismiss)
        {
            HideAndDisable();
        }
        else if (btn.gameObject == buttonAccept)
        {
            if (callback != null)
                callback(true);
            HideAndDisable();
        }
        else if (btn.gameObject == buttonRefuse)
        {
            if (callback != null)
                callback(false);
            HideAndDisable();
        }

    }

    private void HideAndDisable()
    {
        StartCoroutine(HideAndDisable_Corountine());
    }

    private IEnumerator HideAndDisable_Corountine()
    {
        CanvasGroup group = GetComponent<CanvasGroup>();
        if (group != null)
        {
            while (group.alpha > 0f)
            {
                group.alpha -= hideSpeed;
                yield return new WaitForEndOfFrame();
            }
        }
        gameObject.SetActive(false);
    }
}