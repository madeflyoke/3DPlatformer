using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action startGameEvent;

    public static event Action<int> setSceneEvent;   
    public static event Action levelCompleteEvent;
    public static event Action playerDieEvent;

    public static event Action<PlayerAim, Vector3> setPointEvent;  
    public static event Action<float> currentPlayerHealthEvent;

    public static void CallOnStartGame()
    {
        startGameEvent?.Invoke();
    }
    public static void CallOnSetScene(int index)
    {
        setSceneEvent?.Invoke(index);
    }
    public static void CallOnPlayerDie()
    {
        playerDieEvent?.Invoke();
    }
    public static void CallOnCurrentPlayerHealth(float amount)
    {
        currentPlayerHealthEvent?.Invoke(amount);
    }

    public static void CallOnSetPoint(PlayerAim aim, Vector3 pos)
    {
        setPointEvent?.Invoke(aim, pos);
    }

    public static void CallOnLevelComplete()
    {
        levelCompleteEvent?.Invoke();
    }
}
