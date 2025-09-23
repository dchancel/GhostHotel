using UnityEngine;
using System.Collections;

public class TaskController : MonoBehaviour
{


    private void OnEnable()
    {
        StartCoroutine(DelayedAdd());
    }

    private IEnumerator DelayedAdd()
    {
        yield return new WaitForEndOfFrame();
        if (TaskManager.instance != null)
        {
            TaskManager.instance.AddTask(this);
        }
    }

    private void OnDisable()
    {
        if (TaskManager.instance != null)
        {
            TaskManager.instance.RemoveTask(this);
        }
    }
}
