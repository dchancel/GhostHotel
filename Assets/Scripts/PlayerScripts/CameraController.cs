using UnityEngine;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    [SerializeField] private List<Transform> targets = new List<Transform> ();

    [SerializeField] private float zoomCoefficient;

    private Vector3 singleTargetOffset = new Vector3(0f, 5f, -2f);

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        targets.Clear();
    }

    private void LateUpdate()
    {


        if (targets.Count == 0)
        {
            //No players active
            Backup();
        }
        else if (targets.Count == 1)
        {
            //Singleplayer
            Singleplayer();
        }
        else
        {
            //Multiplayer
            Multiplayer();
        }
    }

    public void AddTarget(Transform t)
    {
        targets.Add(t);
    }

    public void RemoveTarget(Transform t)
    {
        targets.Remove(t);
    }

    private void Backup()
    {

    }

    private void Singleplayer()
    {
        transform.position = targets[0].position + singleTargetOffset;
    }

    private void Multiplayer()
    {
        //The actually important part
        Vector3 midpoint = (targets[0].position + targets[1].position)/2f;

        transform.position = midpoint + singleTargetOffset;

        transform.position += transform.forward * -GetZoomLevel();
    }

    private float GetZoomLevel()
    {
        return Mathf.Clamp(Vector3.Distance(targets[0].position, targets[1].position) * zoomCoefficient,1f,100f);
    }
}
