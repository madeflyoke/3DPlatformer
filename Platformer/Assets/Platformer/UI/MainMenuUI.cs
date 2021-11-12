using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button startButton;

    private void Awake()
    {
        startButton.onClick.AddListener(delegate{ startButton.gameObject.transform.DOPunchScale(Vector3.one*0.1f, 0.2f)
                                                  .OnComplete(()=>EventManager.CallOnStartGame());});
    }

}
