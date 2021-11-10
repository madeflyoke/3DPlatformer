using UnityEngine;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;

public class GamePlayUI : MonoBehaviour
{
    [Inject] private RepositoryBase repositoryBase;
    [SerializeField] private Image healthBarFill;

    private void Awake()
    {
        healthBarFill.fillAmount = 1;
    }

    private void OnEnable()
    {
        EventManager.currentPlayerHealthEvent += SetHealthUI;
    }

    private void OnDisable()
    {
        EventManager.currentPlayerHealthEvent -= SetHealthUI;
    }

    private void SetHealthUI(float amount)
    {
        healthBarFill.DOFillAmount((amount * healthBarFill.fillAmount) / repositoryBase.playerInfoObj.maxHealth, 0.5f);      
    }


}
