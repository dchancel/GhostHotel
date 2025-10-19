using UnityEngine;

public class TaskDirector : MonoBehaviour
{
    public Transform target;

    private void LateUpdate()
    {
        transform.LookAt(target, Vector3.up);
    }
}
