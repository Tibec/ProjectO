using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BubbleMgr : MonoBehaviour
{
    public float fadeSpeed;

    public Transform cube;

    public bool fadeIn;
    public bool fadeOut;

    public List<string> availableScenes;

    public string toLoad;
    public bool load;

    void Start(){

    }

    void Update()
    {
        if(load)
        {
            load = false;
            ChangeScene(toLoad);
        }

        if (fadeIn)
        {
            fadeIn = false;
            FadeIn();
        }
        if (fadeOut)
        {
            fadeOut = false;
            FadeOut();
        }
    }

    public void FadeIn()
    {
        MeshRenderer[] faces = cube.GetComponentsInChildren<MeshRenderer>();
        foreach(var f in faces)
        {
            StartCoroutine(ChangeAlpha(f, 1));
        }
    }

    private IEnumerator ChangeAlpha(MeshRenderer r, float targetAlpha)
    {
        float currentAlpha = r.material.color.a;
        while (Mathf.Abs(currentAlpha - targetAlpha) > 2f/255f)
        {
            Color current = r.material.color;
            current.a = Mathf.Lerp(current.a, targetAlpha, fadeSpeed);
            r.material.color = current;
            currentAlpha = current.a;
            yield return new WaitForEndOfFrame();
        }

        Color c = r.material.color;
        c.a = targetAlpha;
        r.material.color = c;
    }

    public void FadeOut()
    {
        MeshRenderer[] faces = cube.GetComponentsInChildren<MeshRenderer>();
        foreach (var f in faces)
        {
            StartCoroutine(ChangeAlpha(f, 0));
        }
    }



    public void ChangeScene(string scene)
    {
        StartCoroutine(ChangeScene_Coroutine(scene));
    }


    IEnumerator ChangeScene_Coroutine(string scene)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
        FadeIn();
        yield return ao;
        FadeOut();
    }
}
