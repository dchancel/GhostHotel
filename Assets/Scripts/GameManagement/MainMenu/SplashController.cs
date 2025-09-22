using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SplashController : MonoBehaviour
{
    public List<SplashData> splash = new List<SplashData>();

    private SplashData activeSplash;

    private float fadeTime = 0.5f;

    private void Start()
    {
        for(int i = 0; i < splash.Count; i++)
        {
            SplashClear(splash[i]);
        }
        SplashOn(0);
    }

    public void SplashOn(int index)
    {
        if(activeSplash != null)
        {
            SplashOff(activeSplash);
        }
        activeSplash = splash[index];
        StartCoroutine(SplashFadeIn());
    }

    private IEnumerator SplashFadeIn()
    {
        yield return new WaitForSeconds(activeSplash.delayTime);
        activeSplash.OnStart.Invoke();

        float t = 0f;
        while(t< fadeTime)
        {
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();

            activeSplash.cg.alpha = Mathf.Lerp(0f,1f,t / fadeTime);
        }

        yield return new WaitForSeconds(activeSplash.holdTime);

        activeSplash.OnEnd.Invoke();
    }

    private void SplashOff(SplashData sd)
    {
        StartCoroutine(SplashFadeOut(sd));
    }

    private IEnumerator SplashFadeOut(SplashData sd)
    {
        float t = 0f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();

            sd.cg.alpha = Mathf.Lerp(1f, 0f, t / fadeTime);
        }
    }

    private void SplashClear(SplashData sd)
    {
        sd.cg.alpha = 0f;
    }

    public void Utility_Load(string s)
    {
        LoadManager.instance.Load(s);
    }
}

[System.Serializable]
public class SplashData
{
    public CanvasGroup cg;
    public float delayTime;
    public float holdTime;
    public UnityEngine.Events.UnityEvent OnStart;
    public UnityEngine.Events.UnityEvent OnEnd;
}
