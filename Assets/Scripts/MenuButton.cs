using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(AudioSource))]
public class MenuButton : MonoBehaviour {

    MenuData mgr;
    public GameObject eventHandler;

    public AudioClip hoverClip;
    public AudioClip clickClip;

    private AudioSource player;

    private GameObject hovered;
    private bool pressed;
    private Graphic[] buttonContent;

    private bool selected;

    private Color normalColor = Color.white;
    private Color hoverColor = new Color(0.58f, 0.78f, 1, 1);
    private Color pressedColor = new Color(0.6f, 0.6f, 0.6f, 1);
    private Color selectedColor = new Color(0.58f, 1, 0.78f, 1);

    private float graceTimerDuration = 2f; // Allow 2 sec before triggering on click if the go has just been activated
    private float graceTimer = 0;

    private bool started = false;

    // Use this for initialization
    void Start () {
        mgr = GetComponentInParent<MenuData>();
        if (mgr == null)
            Debug.LogError("Impossible de trouver le gestionnaire de menu. Le bouton : " + name + " ne fonctionnera pas.");

        hovered = null;
        pressed = false;
        buttonContent = GetComponentsInChildren<Graphic>();
        player = GetComponent<AudioSource>();
        started = true;
    }
	
	// Update is called once per frame
	void Update () {
        graceTimer -= Time.deltaTime;
    }

    private void OnEnable()
    {
        graceTimer = graceTimerDuration;

        if (!started)
            return;

        hovered = null;
        pressed = selected = false;
        UpdateColor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!mgr.Enabled)
                return;
            if (hovered != other.gameObject)
            {
                hovered = other.gameObject;
                PlaySound(true);
            }
            else
            {
                if (graceTimer <= 0)
                {

                    if (eventHandler != null)
                    {
                        eventHandler.SendMessage("OnClick", this);
                    }
                    else // (eventHandler == null)
                    {
                        SendMessage("OnClick", this);
                    }
                }
                pressed = true;
                PlaySound(false);
            }
            UpdateColor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!mgr.enabled)
                return;
            if (pressed)
                pressed = false;
            else if (hovered == other.gameObject)
                hovered = null;

            UpdateColor();
        }
    }

    private void UpdateColor()
    {
        if(selected)
        {
            foreach (Graphic g in buttonContent)
                g.color = selectedColor;
        }
        else if(pressed)
        {
            foreach (Graphic g in buttonContent)
                g.color = pressedColor;
        }
        else if (hovered != null)
        {
            foreach (Graphic g in buttonContent)
                g.color = hoverColor;
        }
        else
        {
            foreach (Graphic g in buttonContent)
                g.color = normalColor;
        }
    }

    public void SetSelected(bool selected)
    {
        this.selected = selected;
        UpdateColor();
    }

    private void PlaySound(bool hover)
    {
        if (graceTimer <= 0)
        {
            if (player.isPlaying)
                player.Stop();
            if (hover)
                player.clip = hoverClip;
            else
                player.clip = clickClip;

            player.Play();
        }
    }
}
