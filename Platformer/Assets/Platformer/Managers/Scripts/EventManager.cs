using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action<GameState> requestGameStateEvent;
    public static event Action<bool> gamePauseEvent;

    public static event Action<PlayerAim, Vector3> setPointEvent;  
    public static event Action<float> currentPlayerHealthEvent;

    public static event Action<Ray> playerInputMoveEvent;
    public static event Action<float> playerGetDamageEvent;
    public static event Action playerAttackEvent;

    public static void CallOnRequestGameState(GameState state)
    {
        requestGameStateEvent?.Invoke(state);
    }
    public static void CallOnGamePause(bool isPaused)
    {
        gamePauseEvent?.Invoke(isPaused);
    }

    public static void CallOnPlayerGetDamage(float damage)
    {
        playerGetDamageEvent?.Invoke(damage);
    }
    public static void CallOnPlayerAttack()
    {
        playerAttackEvent?.Invoke();
    }
    public static void CallOnPlayerInputMove(Ray ray)
    {
        playerInputMoveEvent?.Invoke(ray);
    }
    public static void CallOnCurrentPlayerHealth(float amount)
    {
        currentPlayerHealthEvent?.Invoke(amount);
    }

    public static void CallOnSetPoint(PlayerAim aim, Vector3 pos)
    {
        setPointEvent?.Invoke(aim, pos);
    }

}
