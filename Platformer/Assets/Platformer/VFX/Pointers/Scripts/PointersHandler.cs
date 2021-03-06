using UnityEngine;
using DG.Tweening;
using Zenject;

public class PointersHandler : MonoBehaviour
{
    [Inject] private RepositoryBase repositoryBase;

    [SerializeField] private AudioClip pointerSFX;
    [SerializeField] private float pointDropTime=0.15f;
    [SerializeField] private GameObject MovePointer;
    [SerializeField] private GameObject EnemyPointer;
    [SerializeField] private GameObject InteractPointer;
    private AudioSource audioSource;
    public GameObject movePointer { get; private set; }
    public GameObject enemyPointer { get; private set; }
    public GameObject interactPointer { get; private set; }

    private GameObject currentPointer;
    private Vector3 defaultSize;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        movePointer = Instantiate(MovePointer, transform, false);
        movePointer.SetActive(false);
        enemyPointer = Instantiate(EnemyPointer, transform, false);
        enemyPointer.SetActive(false);
        interactPointer = Instantiate(InteractPointer, transform, false);
        interactPointer.SetActive(false);
        defaultSize = transform.localScale;
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
        transform.DOKill();
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
        transform.position = pos;
        transform.localScale = defaultSize;
        currentPointer.SetActive(true);      
        transform.DOPunchScale(Vector3.one * 0.3f, pointDropTime);
        audioSource.PlayOneShot(pointerSFX, repositoryBase.playerSettingsObj.envVolume*1.5f);

    }
  
}
