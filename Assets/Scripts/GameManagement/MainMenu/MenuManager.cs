using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private List<CanvasGroup> menuGroup = new List<CanvasGroup>();
    [SerializeField] private float transitionTime = 0.3f;

    private CanvasGroup activeCanvas;

    private Vector3 SMALL_SCALE = Vector3.one * 0.9f;
    private Vector3 LARGE_SCALE = Vector3.one * 1.05f;
    private Vector3 NORMAL_SCALE = Vector3.one;

    private const float FIRST_THRESHOLD = 0.8f;
    private const float LAST_THRESHOLD = 1.0f;

    private void Start()
    {
        if(menuGroup.Count > 0)
        {
            SetMenu(menuGroup[0]);
        }
    }

    public void SetMenu(CanvasGroup cg)
    {
        if(activeCanvas != null)
        {
            FadeOut(activeCanvas);
        }
        activeCanvas = cg;
        FadeIn(activeCanvas);

    }

    private void FadeIn(CanvasGroup cg)
    {
        cg.blocksRaycasts = true;
        cg.interactable = true;
        StartCoroutine(DoFadeIn(cg));
    }

    private IEnumerator DoFadeIn(CanvasGroup cg)
    {
        float t = 0f;
        cg.alpha = 0f;

        while(t < transitionTime)
        {
            t += Time.deltaTime;
            
            if(t <= (transitionTime * FIRST_THRESHOLD))
            {
                //Fade In
                cg.alpha = Mathf.Lerp(0f, 1f, t / (transitionTime * FIRST_THRESHOLD));

                //Enlarge
                cg.transform.localScale = Vector3.Lerp(SMALL_SCALE,LARGE_SCALE,t/(transitionTime * FIRST_THRESHOLD));
            }
            else
            {
                //Shrink to Normal
                cg.transform.localScale = Vector3.Lerp(LARGE_SCALE, NORMAL_SCALE, (t - (transitionTime * FIRST_THRESHOLD)) / ((transitionTime * LAST_THRESHOLD) - (transitionTime * FIRST_THRESHOLD)));
            }

            yield return new WaitForEndOfFrame();

        }
    }

    private void FadeOut(CanvasGroup cg)
    {
        cg.blocksRaycasts = false;
        cg.interactable = false;
        StartCoroutine(DoFadeOut(cg));
    }

    private IEnumerator DoFadeOut(CanvasGroup cg)
    {
        float t = 0f;
        cg.alpha = 0f;

        while (t < transitionTime)
        {
            t += Time.deltaTime;

            if (t <= (transitionTime * FIRST_THRESHOLD))
            {

                //Enlarge
                cg.transform.localScale = Vector3.Lerp(NORMAL_SCALE, LARGE_SCALE, t / (transitionTime * FIRST_THRESHOLD));
            }
            else
            {
                //Fade Out
                cg.alpha = Mathf.Lerp(1f, 0f, (t - (transitionTime * FIRST_THRESHOLD)) / ((transitionTime * LAST_THRESHOLD) - (transitionTime * FIRST_THRESHOLD)));
                //Shrink to Normal
                cg.transform.localScale = Vector3.Lerp(LARGE_SCALE, SMALL_SCALE, (t - (transitionTime * FIRST_THRESHOLD)) / ((transitionTime * LAST_THRESHOLD) - (transitionTime * FIRST_THRESHOLD)));
            }

            yield return new WaitForEndOfFrame();

        }
        yield return null;
    }

    public void Utility_Load(string s)
    {
        LoadManager.instance.Load(s);
    }
}
