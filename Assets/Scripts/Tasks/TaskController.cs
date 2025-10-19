using UnityEngine;
using System.Collections;
using UnityEngine.Events;


public class TaskController : MonoBehaviour
{
    public Collider taskLocation;

    public UnityEvent OnPrimary;
    public UnityEvent OnSecondary;
    public UnityEvent OnTaskBegin;
    public UnityEvent OnTaskComplete;

    protected byte containerFullness;
    protected byte containerSize;

    private void OnEnable()
    {
        StartCoroutine(DelayedAdd());
        Setup();
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

    protected virtual void Setup()
    {
        
    }

    public virtual void DoPrimary()
    {
        OnPrimary.Invoke();
    }

    public virtual void DoSecondary()
    {
        OnSecondary.Invoke();
    }

    protected void TaskBegin()
    {
        OnTaskBegin.Invoke();
    }

    protected void TaskComplete()
    {
        OnTaskComplete.Invoke();
    }
}
