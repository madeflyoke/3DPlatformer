using UnityEngine;
using Zenject;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject gamePlayScreen;
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private GameObject levelCompleteScreen;
    //[SerializeField] private GameObject endGameScreen;

    private void Awake()
    {
        SetScreen(0);
    }

    private void OnEnable()
    {
        //EventManager.playerDieEvent += SetEndGameScreen;
        EventManager.setSceneEvent += SetScreen;
        EventManager.levelCompleteEvent += LevelCompleteScreen;
    }
    private void OnDisable()
    {
        //EventManager.playerDieEvent -= SetEndGameScreen;
        EventManager.setSceneEvent -= SetScreen;
        EventManager.levelCompleteEvent -= LevelCompleteScreen;

    }

    private void LevelCompleteScreen()
    {
        levelCompleteScreen.SetActive(true);
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

    private void DisableScreens()
    {
        mainMenuScreen.SetActive(false);
        gamePlayScreen.SetActive(false);
        levelCompleteScreen.SetActive(false);
        //endGameScreen.SetActive(false);
    }

}
