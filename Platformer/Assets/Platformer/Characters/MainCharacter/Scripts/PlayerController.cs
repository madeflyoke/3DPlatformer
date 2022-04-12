using UnityEngine;
using UnityEngine.AI;
using Zenject;
using System;
using System.Collections;
using UnityEngine.EventSystems;
public enum PlayerState
{
    Idle,
    Moving,
    Attack,
    Died
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
    public static event Action<PlayerState> playerCurrentStateEvent;

    [Inject] private RepositoryBase repositoryBase;
    [SerializeField] private AudioClip attackSFX;

    private PlayerState currentPlayerState;
    private PlayerAim currentPlayerAim;
    private string currentAnimationName;
    private float currentHealth;

    private NavMeshAgent agent;
    private Animator animator;
    private AudioSource audioSource;

    private Collider currentRayPointCollider;
    private Vector3 newPos; //correct position to go
    private Enemy currentEnemy;

    private float prevTime;
    private bool isInputsEnable;

    private void Awake()
    {
        currentPlayerState = PlayerState.Idle;
        audioSource = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentHealth = repositoryBase.playerInfoObj.maxHealth;
    }

    private void Start()
    {
        audioSource.volume = repositoryBase.playerSettingsObj.characterVolume;
    }

    private void OnEnable()
    {
        EventManager.playerGetDamageEvent += GetDamage;
        EventManager.playerAttackEvent += Attack;
        EventManager.gamePauseEvent += GamePauseLogic;
    }

    private void OnDisable()
    {
        EventManager.playerGetDamageEvent -= GetDamage;
        EventManager.playerAttackEvent -= Attack;
        EventManager.gamePauseEvent += GamePauseLogic;
    }
    private void GamePauseLogic(bool isGamePaused)
    {
        isInputsEnable = isGamePaused ? false : true;
    }

    private void Update()
    {
        if (isInputsEnable)
        {
            if (
#if PLATFORM_STANDALONE_WIN || UNITY_EDITOR
                Input.GetKeyDown(KeyCode.Mouse0)
#else
                Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended
#endif
               )
            {
                FindPoint();
            }
            ValidateCurrentBehaviour();
        }
    }

    private void ValidateCurrentBehaviour()
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
#if PLATFORM_STANDALONE_WIN || UNITY_EDITOR
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#else
        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
#endif
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance: 100f, layerMask: -1, QueryTriggerInteraction.Ignore))
        {
            if (
#if PLATFORM_STANDALONE_WIN || UNITY_EDITOR
                EventSystem.current.IsPointerOverGameObject() == false)
#else
                EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)==false)
#endif
            {
                if (currentRayPointCollider != null && hit.collider.gameObject == currentRayPointCollider.gameObject)
                    return; //checking for not multiple spamming
                currentRayPointCollider = hit.collider;
                switch (currentRayPointCollider.transform.gameObject.layer)
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
                            EventManager.CallOnSetPoint(aim, currentRayPointCollider.bounds.center);
                            break;
                        case PlayerAim.Ground:
                            EventManager.CallOnSetPoint(aim, newPos);
                            break;

                        case PlayerAim.Enemy:
                            EventManager.CallOnSetPoint(aim, new Vector3(currentRayPointCollider.transform.position.x,
                                                                         currentRayPointCollider.bounds.max.y + 0.5f,
                                                                          currentRayPointCollider.transform.position.z));
                            break;
                    }
                    currentPlayerState = PlayerState.Moving;
                }
                else
                    return;
                break;
            case PlayerState.Attack:
                switch (aim)
                {
                    case PlayerAim.Enemy:
                        currentPlayerState = PlayerState.Attack;
                        currentEnemy = currentRayPointCollider.transform.root.GetComponent<Enemy>();
                        break;
                }
                break;
            case PlayerState.Died:
                currentPlayerState = PlayerState.Died;
                StartCoroutine(Die());
                return;
        }
        playerCurrentStateEvent?.Invoke(currentPlayerState);
    }
    private bool IsValidPath(PlayerAim aim)
    {
        if (NavMesh.SamplePosition(currentRayPointCollider.transform.position, out NavMeshHit hit, 2f, 1)) //get the nearest navpoint
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
                                                      Quaternion.LookRotation(currentEnemy.transform.position - transform.position),
                                                                  Time.deltaTime * agent.angularSpeed / 2); //turn to enemy
                    return true;
                }
                float distanceToEnemy = (currentRayPointCollider.transform.position - transform.position).magnitude;
                if (distanceToEnemy <= repositoryBase.playerInfoObj.attackRange)
                {
                    agent.ResetPath();
                    SetCurrentBehaviour(PlayerState.Attack, PlayerAim.Enemy);
                }
                return true;
        }
        return false;
    }

    public void GetDamage(float damage)
    {
        currentHealth -= damage;
        EventManager.CallOnCurrentPlayerHealth(currentHealth);
        if (currentHealth <= 0)
        {
            SetCurrentBehaviour(PlayerState.Died, PlayerAim.None);
            return;
        }
        animator.SetTrigger("GetHit");
    }

    private IEnumerator Die()
    {
        animator.SetTrigger("Die");
        gameObject.layer = 6;
        isInputsEnable = false;
        yield return new WaitForSeconds(2f);
        EventManager.CallOnRequestGameState(GameState.EndGame);
    }

    private void Attack()
    {
        if (Time.time > prevTime + repositoryBase.playerInfoObj.attackRate)
        {
            animator.SetFloat("AttackNumber", UnityEngine.Random.Range(0f, 1f));
            animator.SetTrigger("Attack");
            prevTime = Time.time;
        }
    }

    private void Hit() //animator controller
    {
        currentEnemy.GetDamage(repositoryBase.playerInfoObj.damage);
        audioSource.PlayOneShot(attackSFX);
    }
}