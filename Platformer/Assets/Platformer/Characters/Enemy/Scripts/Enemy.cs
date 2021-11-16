using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Zenject;


[RequireComponent(typeof(Animator))]
public abstract class Enemy : MonoBehaviour
{
    [Inject] private RepositoryBase repositoryBase;
    [Inject] protected PlayerController player;

    [SerializeField] private ParticleSystem deathEffect;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float damage;
    [SerializeField] protected float attackRange = 0.5f;
    [SerializeField] protected float attackRate;
    [SerializeField] protected AudioClip attackSFX;
    protected float currentHealth;
    protected Animator animator;
    protected AudioSource audioSource;
    private bool isInViewRange;
    protected float prevTime;


    protected NavMeshObstacle navObstacle;
    [HideInInspector] public bool haveLootItem;

    private void Awake()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        navObstacle = GetComponent<NavMeshObstacle>();
    }
    private void Start()
    {
        audioSource.volume = repositoryBase.playerSettingsObj.enemyVolume;
    }

    private void Update()
    {
        if (isInViewRange)
        {
            CheckDistanceToPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer ==3)//player
        {
            isInViewRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3)//player
        {
            isInViewRange = false;
        }
    }

    protected virtual void CheckDistanceToPlayer() {}
    protected virtual void Hit() { }
    protected virtual void Attack() { }


    public void GetDamage(float value)
    {
        currentHealth -= value;
        if (currentHealth == 0)
            Die();
        animator.SetTrigger("GetHit");
    }

    private void Die()
    {
        isInViewRange = false;
        animator.SetTrigger("Die");
        gameObject.layer = 6;//ground layer
        navObstacle.enabled = false;
        StartCoroutine(DieEffect());
    }

    private IEnumerator DieEffect()
    {
        yield return new WaitForSeconds(3f);
        if (haveLootItem)
            GetComponent<InteractItemDrop>().DropItem();
        Destroy(gameObject, 0.1f);
        Instantiate(deathEffect, transform.position, Quaternion.identity).Play();
    }

}
