using System.Collections;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(Animator))]
public abstract class Enemy:MonoBehaviour 
{
    [SerializeField] private ParticleSystem deathEffect;
    [SerializeField] private float MaxHealth;
    [SerializeField] private float Damage;
    private float currentHealth;
    private Animator animator;
    
    private void Awake()
    {
        currentHealth = MaxHealth;
        animator = GetComponent<Animator>();
    }

    public void GetDamage(float value)
    {
        
        currentHealth -= value;
        if (currentHealth==0)
            Die();
        animator.SetTrigger("GetHit");
        Debug.Log(currentHealth);
    }

    private void Die()
    {
        animator.SetTrigger("Die");
        gameObject.layer = 6;//ground layer
        GetComponent<NavMeshObstacle>().enabled = false;
        StartCoroutine(DieEffect());
    }

    private IEnumerator DieEffect()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject,0.1f);
        Instantiate(deathEffect, transform.position, Quaternion.identity).Play();           
    }

}
