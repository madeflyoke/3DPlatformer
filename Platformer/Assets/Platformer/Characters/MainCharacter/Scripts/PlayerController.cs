using UnityEngine;
using UnityEngine.AI;

enum PlayerState
{
    Idle,
    Moving,
    Attack,
}
public enum PlayerAim
{
    None,
    Ground,
    Enemy,
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private RepositoryBase repositoryBase;
    [SerializeField] private Collider weapon;
    private PlayerState currentPlayerState;
    private PlayerAim currentPlayerAim;
    private string currentAnimationName;
    private NavMeshAgent agent;
    private Animator animator;

    private Vector3 newPos;
    private RaycastHit currentRayPoint;

    private void Awake()
    {
        currentPlayerState = PlayerState.Idle;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)/*&&currentPlayerState==PlayerState.Idle*/)//can change direction while moves?
        {
            FindPoint();
        }
    }
    private void FixedUpdate()
    {
        switch (currentPlayerState)
        {
            case PlayerState.Moving:
                if (!agent.pathPending)
                    CheckPos(currentPlayerAim);
                break;
            case PlayerState.Attack:
                Attack();
                break;
            default:
                break;
        }

    }

    private void FindPoint()  //find point and sort for layers
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//MOUSE
        if (Physics.Raycast(ray, out currentRayPoint))
        {
            switch (currentRayPoint.collider.gameObject.layer)
            {
                case 6:
                    SetCurrentBehaviour(PlayerState.Moving, PlayerAim.Ground);
                    break;//ground
                case 7:
                    SetCurrentBehaviour(PlayerState.Moving, PlayerAim.Enemy);
                    break;//enemy

            }
        }
    }


    private void SetCurrentBehaviour(PlayerState state, PlayerAim aim) //set behav depending on state and aim
    {
        currentPlayerAim = aim;
        if (!string.IsNullOrEmpty(currentAnimationName)){ animator.SetBool(currentAnimationName, false); }

        switch (state)
        {
            case PlayerState.Idle:
                switch (aim)
                {
                    case PlayerAim.None:
                        currentPlayerState = PlayerState.Idle;
                        EventManager.CallOnSetPoint(aim, newPos);
                        break;
                }
                break;
            case PlayerState.Moving:
                if (IsValidPath(aim))
                {
                    currentAnimationName = "isMoving";
                    animator.SetBool(currentAnimationName, true);
                    switch (aim)
                    {
                        case PlayerAim.Ground:
                            EventManager.CallOnSetPoint(aim, newPos);
                            break;
                        case PlayerAim.Enemy:
                            EventManager.CallOnSetPoint(aim, new Vector3(currentRayPoint.transform.position.x,
                                                                         currentRayPoint.collider.bounds.max.y + 0.5f,
                                                                          currentRayPoint.transform.position.z));
                            break;
                    }
                    currentPlayerState = PlayerState.Moving;
                }
                break;
            case PlayerState.Attack:
                switch (aim)
                {
                    case PlayerAim.Enemy:
                        Attack();
                        animator.SetBool(currentAnimationName, true);
                        currentAnimationName = "isAttacking";
                        animator.SetBool(currentAnimationName, true);
                        currentPlayerState = PlayerState.Attack;
                        break;
                }
                break;
        }
    }
    private bool IsValidPath(PlayerAim aim)
    {
        if (NavMesh.SamplePosition(currentRayPoint.collider.transform.position, out NavMeshHit hit, 1f, 1)) //get the nearest navpoint
        {
            newPos = hit.position;
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(newPos, path);
            agent.SetPath(path);

            if (CheckPos(aim)) //first check for path status
            {
                return true;
            }
            else
                return false;
        }
        else
        {
            Debug.LogWarning("InvalidPathPoint");
            return false;
        }
    }

    private bool CheckPos(PlayerAim aim) //repeatly check distance to point 
    {
        switch (aim)
        {
            case PlayerAim.Ground:
                if (agent.path.status != NavMeshPathStatus.PathComplete) //is the path reachable
                {
                    agent.ResetPath();
                    SetCurrentBehaviour(PlayerState.Idle, PlayerAim.None);
                    return false;
                }
                else
                {
                    Vector3 direction = newPos - transform.position;
                    if (direction.magnitude <= 0.5f)
                    {
                        SetCurrentBehaviour(PlayerState.Idle, PlayerAim.None);
                        return false;
                    }
                    return true;
                }
            case PlayerAim.Enemy:
                float distanceToEnemy = (currentRayPoint.transform.position - transform.position).magnitude;
                if (distanceToEnemy <= repositoryBase.playerInfoObj.attackRange)
                {
                    agent.ResetPath();
                    SetCurrentBehaviour(PlayerState.Attack, PlayerAim.Enemy);
                    transform.rotation = Quaternion.LookRotation(currentRayPoint.transform.position);//turn towards enemy
                }
                return true;
        }
        return false;
    }

    private void Attack()
    {
        animator.SetFloat("attackNumber", Random.Range(0, 2));
    }

    private void Hit() //animator controller
    {
        weapon.isTrigger = true;
    }



}
