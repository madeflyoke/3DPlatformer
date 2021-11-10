using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button startButton;

    private void Awake()
    {
        startButton.onClick.AddListener(delegate{ /*startButton.gameObject.transform.DOScale(0.95f, 0.2f)*/
                                                EventManager.CallOnStartGame();});
    }

}
