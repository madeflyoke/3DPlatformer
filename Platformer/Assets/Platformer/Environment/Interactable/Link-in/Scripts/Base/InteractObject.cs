using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : MonoBehaviour
{
    [SerializeField] protected GameObject interactObject;
    [SerializeField] protected AudioClip unlockSFX;
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
        if (other.gameObject.layer==3)
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
