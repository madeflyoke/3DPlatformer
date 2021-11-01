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
    Interactable
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private RepositoryBase repositoryBase;
    private PlayerState currentPlayerState;
    private PlayerAim currentPlayerAim;
    private string currentAnimationName;
    private NavMeshAgent agent;
    private Animator animator;

    private RaycastHit currentRayPoint; //ray point on click screen
    private Vector3 newPos; //correct position to go
    private Enemy currentEnemy;
    private float prevTime;

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
        if (currentPlayerState == PlayerState.Attack && Input.GetKeyDown(KeyCode.A)
                    && Time.time > prevTime + repositoryBase.playerInfoObj.attackRate)
        {
            Attack();
            prevTime = Time.time;
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
                CheckPos(currentPlayerAim);
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
            switch (currentRayPoint.transform.root.gameObject.layer)
            {
                case 6:
                    SetCurrentBehaviour(PlayerState.Moving, PlayerAim.Ground);
                    break;//ground
                case 7:
                    SetCurrentBehaviour(PlayerState.Moving, PlayerAim.Enemy);
                    break;//enemy
                case 9:
                    SetCurrentBehaviour(PlayerState.Moving, PlayerAim.Interactable);
                    break;//interactable

            }
        }
    }


    private void SetCurrentBehaviour(PlayerState state, PlayerAim aim) //set behav depending on state and aim
    {
        currentPlayerAim = aim;
        if (!string.IsNullOrEmpty(currentAnimationName)) { animator.SetBool(currentAnimationName, false); }

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
                    currentAnimationName = "IsMoving";
                    animator.SetBool(currentAnimationName, true);
                    switch (aim)
                    {
                        case PlayerAim.Interactable:
                            EventManager.CallOnSetPoint(aim, currentRayPoint.collider.transform.position);
                            break;
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
                        currentPlayerState = PlayerState.Attack;
                        currentEnemy = currentRayPoint.collider.transform.root.GetComponent<Enemy>();
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
            return false;
    }

    private bool CheckPos(PlayerAim aim) //repeatly check the point 
    {
        switch (aim)
        {
            case PlayerAim.Interactable:
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
                if (currentPlayerState == PlayerState.Attack)
                {
                    if (currentEnemy.gameObject.layer == 6)
                    {
                        SetCurrentBehaviour(PlayerState.Idle, PlayerAim.None);
                        return false;
                    }
                    transform.rotation = Quaternion.RotateTowards(transform.rotation,
                                                      Quaternion.LookRotation(currentRayPoint.transform.position - transform.position),
                                                                  Time.deltaTime * agent.angularSpeed / 2); //turn to enemy
                    return true;
                }
                float distanceToEnemy = (currentRayPoint.transform.position - transform.position).magnitude;
                if (distanceToEnemy <= repositoryBase.playerInfoObj.attackRange)
                {
                    agent.ResetPath();
                    SetCurrentBehaviour(PlayerState.Attack, PlayerAim.Enemy);
                }
                return true;
        }
        return false;
    }



    private void Attack()
    {
        animator.SetFloat("AttackNumber", Random.Range(0f, 1f));
        animator.SetTrigger("Attack");
    }

    private void Hit() //animator controller
    {
        currentEnemy.GetDamage(repositoryBase.playerInfoObj.damage);
    }



}
