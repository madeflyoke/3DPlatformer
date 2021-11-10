using UnityEngine;
using Zenject;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject gamePlayScreen;
    [SerializeField] private GameObject mainMenuScreen;
    //[SerializeField] private GameObject endGameScreen;

    private void Awake()
    {
        SetScreen(0);
    }

    private void OnEnable()
    {
        //EventManager.playerDieEvent += SetEndGameScreen;
        EventManager.setSceneEvent += SetScreen;
    }
    private void OnDisable()
    {
        //EventManager.playerDieEvent -= SetEndGameScreen;
        EventManager.setSceneEvent -= SetScreen;
    }

    private void SetScreen(int sceneIndex)
    {
        DisableScreens();
        switch (sceneIndex)
        {
            case 0:
                mainMenuScreen.SetActive(true);
                break;
            case 1:
                gamePlayScreen.SetActive(true);
                break;
        }
    }

    //private void SetEndGameScreen()
    //{
    //    endGameScreen.SetActive(true);
    //}

    private void DisableScreens()
    {
        mainMenuScreen.SetActive(false);
        gamePlayScreen.SetActive(false);
        //endGameScreen.SetActive(false);
    }

}
