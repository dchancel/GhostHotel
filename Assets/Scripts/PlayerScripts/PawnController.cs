using UnityEngine;

public enum MoveSet { ghost,standard}

public class PawnController : MonoBehaviour
{
    [SerializeField] private MoveSet moveSet;
    [SerializeField] private float speed;
    [SerializeField] private bool controlRotation;

    private PlayerController controller;

    private Rigidbody rb;

    private TaskController tc;

    private void Awake()
    {
        if(moveSet != MoveSet.ghost)
        {
            rb = GetComponent<Rigidbody>();
        }

        if (GetComponent<TaskController>())
        {
            tc = GetComponent<TaskController>();
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

    public void SpawnPossession(GameObject newPossession)
    {
        if (!newPossession.GetComponent<PawnController>())
        {
            Debug.LogError("No Pawn Controller Found! Aborting.");
            return;
        }

        GameObject go = Instantiate(newPossession, transform.position, Quaternion.identity);
        go.GetComponent<PawnController>().Possess(controller);
        EndPossession();
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
            transform.position += moveDir * MoveSpeed() * RotationInfluence(moveInput) * Time.deltaTime;
        }
        else
        {
            rb.MovePosition(transform.position + (moveDir * MoveSpeed() * RotationInfluence(moveInput) * Time.deltaTime));
        }
    }

    private float RotationInfluence(Vector2 moveInput)
    {
        if (!controlRotation || moveInput == Vector2.zero)
        {
            return 1f;
        }
        Vector3 lookTarget = transform.position + new Vector3(moveInput.x, 0f, moveInput.y);

        Vector3 lookPos = transform.position + transform.forward;

        if(Vector3.Distance(lookTarget,lookPos) >= Vector3.Distance(lookTarget,transform.position) * 1.95f)
        {
            lookTarget = transform.position + transform.right;
        }

        Vector3 lookTowards = Vector3.MoveTowards(lookPos, lookTarget, Time.deltaTime * speed * 3f);

        transform.LookAt(lookTowards,Vector3.up);

        return Mathf.Lerp(1f,0f, Vector3.Distance(lookTarget, lookPos));
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

        if(tc != null)
        {
            tc.DoPrimary();
        }
    }

    public void DoSecondary()
    {
        if(moveSet == MoveSet.ghost)
        {

        }
        else
        {
            ReleasePawn();
        }

        if (tc != null)
        {
            tc.DoSecondary();
        }
    }

    public void ReleasePawn()
    {
        controller.ReleasePawn(); //return to ghost
    }
}
