using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action<PlayerAim, Vector3> setPointEvent;

    public static void CallOnSetPoint(PlayerAim aim, Vector3 pos)
    {
        setPointEvent?.Invoke(aim, pos);
    }
}
