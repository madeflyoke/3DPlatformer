using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Zenject;


[RequireComponent(typeof(Animator))]
public abstract class Enemy:MonoBehaviour 
{
    [Inject] private RepositoryBase repositoryBase;
    [SerializeField] private ParticleSystem deathEffect;
    [SerializeField] private float MaxHealth;
    [SerializeField] private float Damage;
    [SerializeField] private AudioClip getDamageSFX;
    private float currentHealth;
    private Animator animator;
    private AudioSource audioSource;
   

    private NavMeshObstacle navObstacle;
    [HideInInspector] public bool haveLootItem;
    
    private void Awake()
    {
        currentHealth = MaxHealth;
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        navObstacle = GetComponent<NavMeshObstacle>();
        
    }
    private void Start()
    {
        audioSource.volume = repositoryBase.playerSettingsObj.enemyVolume;
    }

    public void GetDamage(float value)
    {       
        currentHealth -= value;
        if (currentHealth==0)
            Die();
        animator.SetTrigger("GetHit");
        audioSource.PlayOneShot(getDamageSFX);
    }

    private void Die()
    {
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
        Destroy(gameObject,0.1f);
        Instantiate(deathEffect, transform.position, Quaternion.identity).Play();           
    }

}
