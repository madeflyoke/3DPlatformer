using UnityEngine;
using UnityEngine.AI;
using Zenject;

[RequireComponent(typeof(NavMeshObstacle))]
public class Obstacle: MonoBehaviour
{
    [Inject] private RepositoryBase repositoryBase;
    [SerializeField] protected float animationUnlockTime=1f;
    [SerializeField] protected AudioClip unlockSFX;
    protected AudioSource audioSource;
    protected NavMeshObstacle obstacle;
    protected bool isLocked = true;

    protected void Awake()
    {
        obstacle = GetComponent<NavMeshObstacle>();
        audioSource = GetComponent<AudioSource>();      
    }
    private void Start()
    {
        audioSource.volume = repositoryBase.playerSettingsObj.envVolume;
    }
    public virtual void Unlock() { }



}
