using System.Collections.Generic;
using UnityEngine;



public class PointersHandler : MonoBehaviour
{
    [SerializeField] private GameObject MovePointer;
    [SerializeField] private GameObject EnemyPointer;
    public GameObject movePointer { get; private set; }
    public GameObject enemyPointer { get; private set; }
    private GameObject currentPointer;

    private void Awake()
    {
        movePointer = Instantiate(MovePointer, transform, false);
        movePointer.SetActive(false);
        enemyPointer = Instantiate(EnemyPointer, transform, true);
        enemyPointer.SetActive(false);
    }
    private void OnEnable()
    {
        EventManager.setPointEvent += SetPointBehaviour;
    }

    private void OnDisable()
    {
        EventManager.setPointEvent -= SetPointBehaviour;
    }

    private void SetPointBehaviour(PlayerAim aim, Vector3 pos)
    {
        currentPointer?.SetActive(false);
        switch (aim)
        {
            case PlayerAim.None:
                currentPointer?.SetActive(false);
                return;
            case PlayerAim.Ground:
                currentPointer = movePointer;
                break;
            case PlayerAim.Enemy:
                currentPointer = enemyPointer;
                break;
        }
        transform.position = pos;
        currentPointer.SetActive(true);        
    }
  
}
