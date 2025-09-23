using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

    private bool isPaused = false;
    private bool isServing = false;

    private float serviceTime = 0f;
    private float serviceLength = 3f * 60f;

    private void Start()
    {
        StartSetup();
    }

    private void StartSetup()
    {
        isServing = false;
    }

    private void StartService()
    {
        isServing = true;
    }

    private void StartProgression()
    {
        isServing = false;
    }

    private void Update()
    {
        ServiceTimer();
    }

    private void ServiceTimer()
    {
        if (isPaused || !isServing)
        {
            return;
        }

        if(serviceTime < serviceLength)
        {
            serviceTime += Time.deltaTime;
        }
        else
        {
            EndTimer();
        }

    }

    private void EndTimer()
    {
        StartProgression();
    }

}