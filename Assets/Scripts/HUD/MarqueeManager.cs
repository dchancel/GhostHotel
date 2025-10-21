using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MarqueeManager : MonoBehaviour
{
    public List<MarqueeDisplay> marqueeDisplays = new List<MarqueeDisplay>();

    public int soulDelta = 0;

    private Coroutine soulRoutine;

    public void ModifySouls(int amount)
    {
        soulDelta += amount;
        if(soulRoutine == null)
        {
            soulRoutine = StartCoroutine(DisplayUpdate());
        }
    }

    private IEnumerator DisplayUpdate()
    {
        while(soulDelta != 0)
        {
            yield return new WaitForEndOfFrame();

        }
        soulRoutine = null;
    }
}
