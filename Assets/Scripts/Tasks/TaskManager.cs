using UnityEngine;
using System.Collections.Generic;

public class TaskManager : MonoBehaviour
{
    public static TaskManager instance;

    [SerializeField] private List<TaskController> tasks = new List<TaskController>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void AddTask(TaskController task)
    {
        if (tasks.Contains(task))
        {
            return;
        }

        tasks.Add(task);
    }

    public void RemoveTask(TaskController task)
    {
        if (tasks.Contains(task))
        {
            tasks.Remove(task);
        }
    }
}
