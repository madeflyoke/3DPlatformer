using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class DoorObstacle : MonoBehaviour, IObstacle
{
    [SerializeField] private GameObject PartToUnlock;
    private NavMeshObstacle Obstacle;
    private bool isLocked = true;

    public bool locking { get => isLocked; set => isLocked =value; }
    public GameObject partToUnlock =>PartToUnlock;
    public NavMeshObstacle obstacle => Obstacle;

    private void Awake()
    {
        Obstacle = GetComponent<NavMeshObstacle>();
    }

    public void Unlock()
    {
        partToUnlock.transform.DORotate(new Vector3(0f, 90f, 0f), 1f);
        Obstacle.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("triger");
        if (!isLocked)
        {
            if (other.CompareTag("Player"))
            {
                Unlock();
            }
        }
    }
}
