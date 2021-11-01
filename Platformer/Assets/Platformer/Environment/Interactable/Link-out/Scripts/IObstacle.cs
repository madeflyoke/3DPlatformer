using UnityEngine;
using UnityEngine.AI;

public interface IObstacle
{
    public GameObject partToUnlock { get;}
    public NavMeshObstacle obstacle { get; }
    public bool locking { get; set; }
    public void Unlock();
 

}
