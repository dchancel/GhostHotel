using UnityEngine;
using System.Collections;

public enum HotelEffects { none,trashcan}

public class HotelEffect : MonoBehaviour
{
    public bool isLocal;
    public float influence; //influences are cardinal directions, not a radius. so a 5 x 5 box, not a 5 radius
    public HotelEffects effect;

    private void OnEnable()
    {
        if (!isLocal)
        {
            influence = Mathf.Infinity;
        }

        StartCoroutine(DelayedAdd());
    }

    private IEnumerator DelayedAdd()
    {
        yield return new WaitForEndOfFrame();
        LevelManager.instance.hotelEffects.Add(this);
    }

    private void OnDisable()
    {
        LevelManager.instance.hotelEffects.Remove(this);
    }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawLine(transform.position + (Vector3.forward * influence), transform.position + (Vector3.forward * -influence), Color.green);
        Debug.DrawLine(transform.position + (Vector3.right * influence), transform.position + (Vector3.right * -influence), Color.red);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(influence*2f,1f,influence*2f));
    }
}
