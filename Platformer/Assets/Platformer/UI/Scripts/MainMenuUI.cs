using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button startButton;

    private void StartButtonListener()
    {
        startButton.onClick.RemoveListener(StartButtonListener);
        startButton.gameObject.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f)
                              .OnComplete(() => EventManager.CallOnRequestGameState(GameState.Start));
    }

    public void Init()
    {
        startButton.onClick.AddListener(StartButtonListener);
    }

}
