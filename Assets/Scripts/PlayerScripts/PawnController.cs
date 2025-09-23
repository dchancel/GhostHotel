using UnityEngine;

public enum MoveSet { ghost,standard}

public class PawnController : MonoBehaviour
{
    [SerializeField] private MoveSet moveSet;
    [SerializeField] private float speed;

    private PlayerController controller;

    private Rigidbody rb;

    private void Awake()
    {
        if(moveSet != MoveSet.ghost)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    public bool Possess(PlayerController pc)
    {
        if(controller != null)
        {
            return false;
        }
        controller = pc;
        return true;
    }

    public void EndPossession()
    {
        controller = null;
    }

    public bool IsNPC()
    {
        //If no controller is attached, this is an NPC, and is available for NPC control
        return controller == null;
    }

    public void DoMove(Vector2 moveInput)
    {
        Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        if (moveSet == MoveSet.ghost)
        {
            //ghosts do not have colliders, and so do not need to worry about such things
            transform.position += moveDir * MoveSpeed() * Time.deltaTime;
        }
        else
        {
            rb.MovePosition(transform.position + (moveDir * MoveSpeed() * Time.deltaTime));
        }
    }

    private float MoveSpeed()
    {
        //Can be modified by things like holding a sprint button, theoretically
        return speed;
    }

    public void DoPrimary()
    {
        if (moveSet == MoveSet.ghost)
        {
            controller.TryPossess();
        }
    }

    public void DoSecondary()
    {
        if(moveSet == MoveSet.ghost)
        {

        }
        else
        {
            controller.ReleasePawn(); //return to ghost
        }
    }
}
