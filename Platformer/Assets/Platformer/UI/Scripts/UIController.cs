using UnityEngine;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    [SerializeField] private GamePlayUI gamePlayScreen;
    [SerializeField] private MainMenuUI mainMenuScreen;
    [SerializeField] private GameObject levelCompleteScreen;
    [SerializeField] private EndGameUI endGameScreen;
    

    private void Awake()
    {
        DisableScreens();
    }

    private void OnEnable()
    {
        GameStateController.setGameStateEvent += SetScreenByGameState;
    }
    private void OnDisable()
    {
        GameStateController.setGameStateEvent -= SetScreenByGameState;
    }

    private void SetScreenByGameState(GameState state)
    {
        switch (state)
        {
            case GameState.Menu:
                DisableScreens();
                mainMenuScreen.gameObject.SetActive(true);
                mainMenuScreen.Init();
                break;
            case GameState.GamePlay:
                DisableScreens();
                gamePlayScreen.gameObject.SetActive(true);
                gamePlayScreen.Init();
                break;
            case GameState.LevelComplete:
                DisableScreens();
                levelCompleteScreen.SetActive(true);
                break;
            case GameState.EndGame:
                DisableScreens();               
                endGameScreen.gameObject.SetActive(true);
                endGameScreen.Init();
                break;
        }
    }

    private void DisableScreens()
    {
        mainMenuScreen.gameObject.SetActive(false);
        gamePlayScreen.gameObject.SetActive(false);
        levelCompleteScreen.SetActive(false);
        endGameScreen.gameObject.SetActive(false);
    }

}
