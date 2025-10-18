using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;

public class TriggerVolume : MonoBehaviour
{
    private List<GameObject> heldPlayers = new List<GameObject>();

    private bool everyoneIsHere = false;

    private Coroutine triggerTimer;

    [SerializeField] private float triggerTime = 3f;
    [SerializeField] private UnityEvent onTrigger;
    [Header("References")]
    [SerializeField] private UnityEngine.UI.Image fillImage;

    private void Start()
    {
        onTrigger.AddListener(BaseTriggerEvent);
    }

    private void BaseTriggerEvent()
    {
        gameObject.SetActive(false);
    }
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + " entered trigger");
        if (other.GetComponent<PlayerController>())
        {
            heldPlayers.Add(other.gameObject);
        }

        CheckIfEveryoneIsHere();
    }

    public void OnTriggerExit(Collider other)
    {
        if (heldPlayers.Contains(other.gameObject))
        {
            heldPlayers.Remove(other.gameObject);
        }

        CheckIfEveryoneIsHere();
    }

    private void CheckIfEveryoneIsHere()
    {
        if(heldPlayers.Count >= LevelManager.instance.players && !everyoneIsHere)
        {
            everyoneIsHere = true;
            StartCounter();
        }
        else if(heldPlayers.Count < LevelManager.instance.players && everyoneIsHere)
        {
            everyoneIsHere = false;
            StopCounter();
        }
    }

    private void StartCounter()
    {
        if(triggerTimer != null)
        {
            StopCoroutine(triggerTimer);
            triggerTimer = null;
        }

        triggerTimer = StartCoroutine(TriggerRoutine());
    }

    private void StopCounter()
    {
        if (triggerTimer != null)
        {
            StopCoroutine(triggerTimer);
            triggerTimer = null;
            fillImage.fillAmount = 0f;
        }
    }

    private IEnumerator TriggerRoutine()
    {
        float t = 0f;

        while(t < triggerTime)
        {
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
            fillImage.fillAmount = t / triggerTime;
        }

        onTrigger.Invoke();
        StopCounter();
    }
}
