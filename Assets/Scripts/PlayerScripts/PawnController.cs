using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public enum MoveSet { ghost,standard,cart}

public class PawnController : MonoBehaviour
{
    [SerializeField] private MoveSet moveSet;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool controlRotation;

    public UnityEvent OnBeginPossession;
    public UnityEvent OnEndPossession;

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

        if(rotationSpeed == 0f)
        {
            rotationSpeed = speed;
        }
    }

    public bool Possess(PlayerController pc)
    {
        if(controller != null)
        {
            return false;
        }
        controller = pc;
        OnBeginPossession.Invoke();
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
        StartCoroutine(DelayedPossession(go, controller));
        ReleasePawn();
    }

    private IEnumerator DelayedPossession(GameObject go, PlayerController pc)
    {
        yield return new WaitForEndOfFrame();
        pc.Possess(go.GetComponent<PawnController>());
        //go.GetComponent<PawnController>().Possess(pc);
    }

    public void EndPossession()
    {
        controller = null;
        OnEndPossession.Invoke();
    }

    public void SetPawnHeight(float f)
    {
        StartCoroutine(LerpPawnHeight(f));
    }

    private IEnumerator LerpPawnHeight(float f)
    {
        float t = 0f;
        float timer = 0.3f;
        Vector3 starting = transform.position;
        Vector3 ending = new Vector3(transform.position.x,f,transform.position.z);
        GetComponent<Collider>().enabled = false;
        while(t < timer)
        {
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(starting,ending,t/timer);
        }
        rb.linearVelocity = Vector3.zero;
        GetComponent<Collider>().enabled = true;
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
        else if(moveSet == MoveSet.cart)
        {
            if(moveDir == Vector3.zero)
            {
                return;
            }

            Vector3 proxyForward = transform.forward;

            if(Vector3.Distance(transform.position + moveDir,transform.position + transform.forward) > Vector3.Distance(transform.position + moveDir, transform.position - transform.forward))
            {
                Debug.Log("moving backwards");
                proxyForward = -transform.forward;
            }
            RotationInfluence(moveInput, proxyForward);
            rb.MovePosition(transform.position + (proxyForward * MoveSpeed() * Time.deltaTime));
        }
        else
        {
            rb.MovePosition(transform.position + (moveDir * MoveSpeed() * RotationInfluence(moveInput) * Time.deltaTime));
        }
    }

    private float RotationInfluence(Vector2 moveInput, Vector3 forward)
    {
        if(forward == -transform.forward)
        {
            Debug.Log("flipping move input");
            moveInput = -moveInput;
        }

        Vector3 lookTarget = transform.position + new Vector3(moveInput.x, 0f, moveInput.y);

        Vector3 lookPos = transform.position + transform.forward;

        if (Vector3.Distance(lookTarget, lookPos) >= Vector3.Distance(lookTarget, transform.position) * 1.95f)
        {
            lookTarget = transform.position + transform.right;
        }

        Vector3 lookTowards = Vector3.MoveTowards(lookPos, lookTarget, Time.deltaTime * rotationSpeed * 3f);

        transform.LookAt(lookTowards, Vector3.up);

        return Mathf.Lerp(1f, 0f, Vector3.Distance(lookTarget, lookPos));
    }

    private float RotationInfluence(Vector2 moveInput)
    {
        if (!controlRotation || moveInput == Vector2.zero)
        {
            return 1f;
        }

        return RotationInfluence(moveInput, transform.forward);
        
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
