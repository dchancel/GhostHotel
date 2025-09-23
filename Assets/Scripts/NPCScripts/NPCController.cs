using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] private PawnController pawn;

    private void Start()
    {
        pawn = transform.GetChild(0).GetComponent<PawnController>(); //An NPC shall always be composed in the same way, with the pawn as its first child
        //The pawn can be moved independently while it is under control of a ghost, and when that event ends, then the NPCController should snap to that location
        //Always at an offset of 0,0,0
    }

    private void SnapTogether()
    {
        transform.position = pawn.transform.position;
        pawn.transform.localPosition = Vector3.zero;
    }
}
