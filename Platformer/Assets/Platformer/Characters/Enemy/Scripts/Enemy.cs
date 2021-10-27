using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy:MonoBehaviour 
{
    [SerializeField] private float MaxHealth;
    [SerializeField] private float Damage;
    private float currentHealth;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("PlayerAttackSword"))
        {
            Debug.Log("col");
        }
        
    }


}
