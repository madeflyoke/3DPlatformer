using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InteractObject : MonoBehaviour
{
    [Inject] protected RepositoryBase repositoryBase;
    [Inject] protected AudioManager audioManager;

    [SerializeField] protected GameObject interactObject;
    [SerializeField] protected AudioClip unlockSFX;
    protected bool isEnabled = true;
    protected List<Obstacle> partsToUnlock=new List<Obstacle>();
    protected void Awake()
    {
        foreach (Obstacle item in interactObject.GetComponentsInChildren<Obstacle>())
        {
            partsToUnlock.Add(item);
        }
    }
    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer==3&&isEnabled)
        {
            foreach (Obstacle item in partsToUnlock)
            {
                item.Unlock();
            }            
            ObjectDisable();
        }
    }
    protected virtual void ObjectDisable() { }
   



}
