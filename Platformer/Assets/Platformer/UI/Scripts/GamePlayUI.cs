using UnityEngine;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;

public class GamePlayUI : MonoBehaviour
{
    [Inject] private RepositoryBase repositoryBase;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private Button attackButton;

    private void OnEnable()
    {
        EventManager.currentPlayerHealthEvent += SetHealthUI;
        PlayerController.playerCurrentStateEvent += PlayerStateHolder;
        attackButton.onClick.AddListener(AttackButtonClick);
    }
    private void OnDisable()
    {
        EventManager.currentPlayerHealthEvent -= SetHealthUI;
        PlayerController.playerCurrentStateEvent -= PlayerStateHolder;
    }
    private void AttackButtonClick()
    {
        EventManager.CallOnPlayerAttack();
        attackButton.transform.DOKill();
        attackButton.transform.localScale = Vector3.one;
        attackButton.transform.DOPunchScale(Vector3.one *0.2f, 0.3f, vibrato:10);
    }
 
    private void SetHealthUI(float amount)
    {
        healthBarFill.DOFillAmount(amount / repositoryBase.playerInfoObj.maxHealth, 0.5f);
    }

    private void PlayerStateHolder(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Attack:
                attackButton.gameObject.SetActive(true);
                return;
        }
        attackButton.gameObject.SetActive(false);
    }

    public void Init()
    {
        healthBarFill.fillAmount = 1;
        attackButton.gameObject.SetActive(false);
    }
}
