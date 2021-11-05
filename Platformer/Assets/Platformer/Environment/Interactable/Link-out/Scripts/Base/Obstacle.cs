using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshObstacle))]
public class Obstacle: MonoBehaviour
{
    [SerializeField] protected float animationUnlockTime=1f;
    [SerializeField] protected AudioClip unlockSFX;
    protected AudioSource audioSource;
    protected NavMeshObstacle obstacle;
    protected bool isLocked = true;

    protected void Awake()
    {
        obstacle = GetComponent<NavMeshObstacle>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = AudioManager.instance.envVolume;
    }
    public virtual void Unlock() { }



}
