using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action<PlayerAim, Vector3> setPointEvent;
    public static event Action levelCompleteEvent;
    public static event Action<int> changeSceneEvent;

    public static void CallOnChangeScene(int sceneIndex)
    {
        changeSceneEvent?.Invoke(sceneIndex);
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
