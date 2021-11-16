using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] private Button retryButton;
    [SerializeField] private Image labelToAppear;
    [SerializeField] private TMP_Text labelText;
   
    private void RetryButtonListener()
    {
        retryButton.onClick.RemoveListener(RetryButtonListener);
        retryButton.gameObject.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f)
                              .OnComplete(() => EventManager.CallOnRequestGameState(GameState.LevelRestart));
    }

    public void Init()
    {
        retryButton.gameObject.SetActive(false);
        labelToAppear.transform.localScale *= 0.6f;  
        Sequence seq = DOTween.Sequence();
        seq.Append(labelToAppear.DOFade(1f, 5f))
            .Insert(0f,labelText.DOFade(1f,5f))
            .Insert(0f, labelToAppear.transform.DOScale(1f, 5f))
            .OnComplete(()=>retryButton.gameObject.SetActive(true));
        retryButton.onClick.AddListener(RetryButtonListener);

    }
}
