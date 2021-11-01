using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;



public class PointersHandler : MonoBehaviour
{
    [SerializeField] private float pointDropTime=0.15f;
    [SerializeField] private GameObject MovePointer;
    [SerializeField] private GameObject EnemyPointer;
    [SerializeField] private GameObject InteractPointer;
    public GameObject movePointer { get; private set; }
    public GameObject enemyPointer { get; private set; }
    public GameObject interactPointer { get; private set; }

    private GameObject currentPointer;

    private void Awake()
    {
        movePointer = Instantiate(MovePointer, transform, false);
        movePointer.SetActive(false);
        enemyPointer = Instantiate(EnemyPointer, transform, false);
        enemyPointer.SetActive(false);
        interactPointer = Instantiate(InteractPointer, transform, false);
        interactPointer.SetActive(false);
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
        DOTween.Clear();
        currentPointer?.SetActive(false);
        switch (aim)
        {
            case PlayerAim.None:
                return;
            case PlayerAim.Ground:
                currentPointer = movePointer;
                break;
            case PlayerAim.Enemy:
                currentPointer = enemyPointer;
                break;
            case PlayerAim.Interactable:
                currentPointer = interactPointer;
                transform.position = pos;
                currentPointer.SetActive(true);
                return;
        }
        transform.position = pos+Vector3.up;
        currentPointer.SetActive(true);      
        transform.DOMove(pos, pointDropTime);

    }
  
}
