using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public enum GameState
{
    None,
    Menu,
    Start,
    GamePlay,
    LevelComplete,
    LevelRestart,
    EndGame
}

public class GameStateController : MonoBehaviour
{
    public static event Action<GameState> setGameStateEvent;
    public static event Action<int> setSceneEvent;

    [SerializeField] private int targetFps;
    private GameState currentGameState = GameState.None;
    private int currentSceneIndex = 0;

    private void Awake()
    {
        Application.targetFrameRate = targetFps;
    }
    private void Start()
    {
        SetCurrentGameState(GameState.Menu);
    }

    private void OnEnable()
    {
        EventManager.requestGameStateEvent += SetCurrentGameState;
    }

    private void OnDisable()
    {
        EventManager.requestGameStateEvent -= SetCurrentGameState;
    }

    private void SetCurrentGameState(GameState state)
    {
        currentGameState = state;
        setGameStateEvent?.Invoke(currentGameState);
        switch (state)
        {
            case GameState.None:
                break;
            case GameState.Menu:
                break;
            case GameState.Start:
                StartCoroutine(SetScene(1, GameState.GamePlay));
                break;
            case GameState.GamePlay:
                PauseGame(false);
                break;
            case GameState.LevelComplete:
                PauseGame(true);
                //next level logic
                break;
            case GameState.LevelRestart:
                StartCoroutine(SetScene(currentSceneIndex, GameState.GamePlay));
                break;
            case GameState.EndGame:
                PauseGame(true);
                break;
        }
    }
    private IEnumerator SetScene(int index, GameState aimState)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);
        while (operation.progress < 1)
        {
            yield return null;
        }
        currentSceneIndex = index;
        setSceneEvent?.Invoke(currentSceneIndex);
        SetCurrentGameState(aimState);
        yield return null;
    }

    private void PauseGame(bool isPaused)
    {
        EventManager.CallOnGamePause(isPaused);
    }


}
