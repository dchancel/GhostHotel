using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MarqueeManager : MonoBehaviour
{
    public List<MarqueeDisplay> marqueeDisplays = new List<MarqueeDisplay>();

    public int soulDelta = 0;
    private int soulsCache = 0;

    private Coroutine soulRoutine;

    private float slideTimer = 0.3f;
    private Vector2 expandedPosition = new Vector2(0f,0f);
    private Vector2 stowedPosition = new Vector2(0f,0f);

    private Coroutine slideRoutine;

    public void ModifySouls(int amount)
    {
        soulDelta += amount;
        if(soulRoutine != null)
        {
            StopCoroutine(soulRoutine);
        }
        soulRoutine = StartCoroutine(DisplayUpdate());
    }

    public void ShowSouls()
    {
        if(slideRoutine != null)
        {
            StopCoroutine(slideRoutine);
        }
        slideRoutine = StartCoroutine(SlideSouls(expandedPosition));
    }

    public void HideSouls()
    {
        if (slideRoutine != null)
        {
            StopCoroutine(slideRoutine);
        }
        slideRoutine = StartCoroutine(SlideSouls(stowedPosition));
    }

    private IEnumerator SlideSouls(Vector2 endPosition)
    {
        float t = 0f;
        Vector2 startPosition = transform.position;
        while(t < slideTimer)
        {
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;

            transform.position = Vector2.Lerp(startPosition, endPosition, t / slideTimer);
        }
    }

    private IEnumerator DisplayUpdate()
    {
        int startingSouls = soulsCache;
        int incomingSouls = soulDelta;
        int transfer = 0;

        float t = 0f;
        float timer = Mathf.Clamp(incomingSouls / 30f,0f,0.5f); //1 soul takes 0.03 seconds //100 souls takes 3 seconds //So we clamp it to a maximum of 0.5 seconds
        while(t < timer)
        {
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
            transfer = (int)Mathf.Lerp(0,incomingSouls,t/timer);
            soulsCache = startingSouls + transfer;
            soulDelta = incomingSouls - transfer;
            DoDisplay(soulsCache);
        }
    }

    private void DoDisplay(int amount)
    {
        Debug.Log("Souls: " + amount.ToString());
    }
}
