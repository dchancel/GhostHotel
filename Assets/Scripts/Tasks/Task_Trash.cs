using UnityEngine;

public class Task_Trash : TaskContainer
{

    public UnityEngine.Events.UnityEvent trashEvent;

    public void TryTrashEvent()
    {
        if(containerFullness <= 0)
        {
            return;
        }
        else
        {
            trashEvent.Invoke();
        }
    }

    protected override void Setup()
    {
        containerFullness = 1;
        containerSize = 3;
    }

    public void EmptyTrash()
    {
        containerFullness = 0;
    }

    
}
