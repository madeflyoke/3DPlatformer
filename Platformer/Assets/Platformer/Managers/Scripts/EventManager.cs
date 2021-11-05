using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action<PlayerAim, Vector3> setPointEvent;
    public static event Action<int> setSceneEvent;
    public static event Action levelCompleteEvent;

    public static void CallOnSetPoint(PlayerAim aim, Vector3 pos)
    {
        setPointEvent?.Invoke(aim, pos);
    }

    public static void CallOnSetScene(int sceneIndex)
    {
        setSceneEvent?.Invoke(sceneIndex);
    }

    public static void CallOnLevelComplete()
    {
        levelCompleteEvent?.Invoke();
    }
}
