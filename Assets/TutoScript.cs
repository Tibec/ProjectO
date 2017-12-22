using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class TutoScript : MonoBehaviour {

    public Text panel;
    public AudioClip success;

    [Multiline]
    public string hello = "";
    private bool helloOk = false;
    [Multiline]
    public string howToOpenMenu = "";
    private bool howToOpenMenuOk = false;
    [Multiline]
    public string howToAddFurniture = "";
    private bool howToAddFurnitureOk = false;
    [Multiline]
    public string howToCloseMenu = "";
    private bool howToCloseMenuOk = false;


    // Use this for initialization
    void Start () {
        panel.text = hello;
	}
	
	// Update is called once per frame
	void Update () {
		if(FindObjectOfType<MenuManager>() != null && !howToOpenMenuOk && helloOk)
        {
            panel.text = howToAddFurniture;
            PlaySuccessSound();
            howToOpenMenuOk = true;
        }

        MeubleSpawnMgr ms = FindObjectOfType<MeubleSpawnMgr>();
        if (ms != null && ms.transform.childCount > 0 && !howToAddFurnitureOk )
        {
            panel.text = howToCloseMenu;
            PlaySuccessSound();
            howToAddFurnitureOk = true;
        }

        if (FindObjectOfType<MenuManager>() == null && !howToCloseMenuOk && howToOpenMenuOk)
        {
            FindObjectOfType<BubbleMgr>().FadeOut();
            howToCloseMenuOk = true;
            panel.text = "";
        }
    }

    void OnClick(MenuButton btn)
    {
        if(btn.name == "Panel")
        {
            Destroy(btn.gameObject);
            panel.text = howToOpenMenu;
            PlaySuccessSound();
            helloOk = true;
        }
    }

    void PlaySuccessSound()
    {
        AudioSource player = GetComponent<AudioSource>();
        if (player.isPlaying)
            player.Stop();

        player.clip = success;
        player.Play();
    }
}
