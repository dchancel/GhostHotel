using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //PlayerController is an empty gameobject that takes and manages player inputs from various input sources
    //A player will at all times be in control of a "pawn", which then receives that input

    public PawnController pawn;
    public PawnController ghost;

    public Camera playerCamera;

    private bool isActive = false;
    private Vector2 cachedMove;
    private Vector3 cameraOffset = new Vector3(0f,-5f,2.4f);

    private PawnController target;

    private const float GHOST_HEIGHT = 1.5f;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void Possess(PawnController p)
    {
        if (p.Possess(this))
        {
            pawn = p;
            SetGhostActive(false);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void ReleasePawn()
    {
        //do any additional logic for releasing a pawn, like turning off a shader
        if(pawn == ghost)
        {
            return;
        }
        pawn.EndPossession();
        BecomeGhost();
    }

    public void BecomeGhost()
    {
        //Set ghost to proper position
        Vector3 v = pawn.transform.position;
        v.y = GHOST_HEIGHT;
        ghost.transform.position = v;
        pawn = ghost;
        SetGhostActive(true);
    }

    private void SetGhostActive(bool b)
    {
        //These branching conditions are not really needed at this point, but can be expanded on to make sure more things happen when the player stops or starts being a ghost
        if (b)
        {
            ghost.gameObject.SetActive(true);
            ReleasePawn();
            pawn = ghost;
        }
        else
        {
            ghost.gameObject.SetActive(false);
        }
    }

    public void Setup(PlayerData pd)
    {
        if(pd == null)
        {
            return;
        }
        ghost = pd.ghost;
        ghost.Possess(this);
        pawn = ghost;
        isActive = true;
        pd.playerObject.SetActive(true);
        CameraController.Instance.AddTarget(this.transform);
    }

    public void Packout(PlayerData pd)
    {
        isActive = false;
        ReleasePawn();
        if(pd.playerObject == null)
        {
            return;
        }
        pd.playerObject.SetActive(false);
        CameraController.Instance.RemoveTarget(this.transform);
    }

    public void TryPossess()
    {
        if(target == null)
        {
            return;
        }
        Possess(target);
    }

    private void Update()
    {
        if (UIInputHandler.instance != null)
        {

        }

        if (!isActive)
        {
            return;
        }


        if (pawn != null)
        {
            pawn.DoMove(cachedMove);

            if(GroundPosition(pawn.transform.position) != transform.position)
            {
                transform.position = GroundPosition(pawn.transform.position);
            }
        }



    }

    private Vector3 GroundPosition(Vector3 v)
    {
        v.y = 0f;
        return v;
    }

    public void OnMove(InputValue c)
    {
        cachedMove = c.Get<Vector2>();
    }

    public void OnPrimary(InputValue c)
    {
        if (pawn != null)
        {
            pawn.DoPrimary();
        }
        if (UIInputHandler.instance != null)
        {

        }
    }

    public void OnSecondary(InputValue c)
    {
        if (pawn != null)
        {
            pawn.DoSecondary();
        }
        if (UIInputHandler.instance != null)
        {

        }
    }

    public void OnPause(InputValue c)
    {
        //Does not need to reference the pawn

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PawnController>())
        {
            target = other.GetComponent<PawnController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<PawnController>())
        {
            if(target == other.GetComponent<PawnController>())
            {
                target = null;
            }
        }
    }
}
