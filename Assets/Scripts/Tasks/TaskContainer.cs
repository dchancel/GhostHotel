using UnityEngine;

public class TaskContainer : TaskController
{
    public bool IsFull()
    {
        if (containerFullness >= containerSize)
        {
            return true;
        }
        return false;
    }

    public void Add()
    {
        if (IsFull())
        {
            return;
        }
        containerFullness++;
    }
}
