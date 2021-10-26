using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System;

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
    [SerializeField] GameObject testobj;
    [SerializeField] private RepositoryBase repositoryBase;
    private PlayerState currentPlayerState;
    private PlayerAim currentPlayerAim;
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
    }
    private void FixedUpdate()
    {
        if (currentPlayerState == PlayerState.Moving && !agent.pathPending)
        {
            CheckPos(currentPlayerAim);
            Debug.Log(agent.path.status);
        }
    }

    private void SetCurrentBehaviour(PlayerState state, PlayerAim aim)
    {
        currentPlayerAim = aim;
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
                    switch (aim)
                    {
                        case PlayerAim.Ground:
                            EventManager.CallOnSetPoint(aim, newPos);
                            break;
                        case PlayerAim.Enemy:
                            EventManager.CallOnSetPoint(aim, new Vector3(currentRayPoint.transform.position.x, 
                                                                         currentRayPoint.collider.bounds.max.y+0.5f,
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
                        Attack();
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
                animator.SetBool("isMoving", true);
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

    private bool CheckPos(PlayerAim aim)
    {
        switch (aim)
        {
            case PlayerAim.Ground:
                if (agent.path.status != NavMeshPathStatus.PathComplete)
                {
                    animator.SetBool("isMoving", false);
                    agent.ResetPath();
                    SetCurrentBehaviour(PlayerState.Idle, PlayerAim.None);
                    return false;
                }
                else
                {
                    Vector3 direction = newPos - transform.position;
                    if (direction.magnitude <= 0.5f)
                    {
                        animator.SetBool("isMoving", false);
                        SetCurrentBehaviour(PlayerState.Idle, PlayerAim.None);
                        return false;
                    }
                    return true;
                }
            case PlayerAim.Enemy:
                float distanceToEnemy = (currentRayPoint.transform.position - transform.position).magnitude;
                if (distanceToEnemy <= repositoryBase.playerInfoObj.attackRange)
                {
                    animator.SetBool("isMoving", false);
                    agent.ResetPath();
                    SetCurrentBehaviour(PlayerState.Attack, PlayerAim.Enemy);
                }
                return true;
        }
        return false;
        //rotate character

        //float turnAngle = Mathf.Atan2(direction.normalized.x, direction.normalized.z) * Mathf.Rad2Deg;
        //Quaternion rotateTo = Quaternion.Euler(new Vector3(0, turnAngle, 0));
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotateTo, Time.deltaTime * 10);


        //transform.position = Vector3.MoveTowards(transform.position, newPos, Time.deltaTime * speed);//moving
        //if (direction.magnitude <= 0.5f)
        //{
        //    movePointClone.SetActive(false);
        //    if (direction.magnitude <= 0.1f)
        //    {
        //        animator.SetBool("isMoving", false);
        //        SetCurrentState(PlayerState.Idle);
        //    }
        //}
    }

    private void Attack()
    {
        animator.SetBool("isAttacking", true);
    }


}
