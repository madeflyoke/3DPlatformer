using UnityEngine;

public class RepositoryBase : MonoBehaviour
{
    PlayerInfo playerInfo = new PlayerInfo();
    public PlayerInfo playerInfoObj => playerInfo;

    PlayerSettings playerSettings = new PlayerSettings();
    public PlayerSettings playerSettingsObj => playerSettings;

    private void Awake()
    {
        playerSettingsObj.Initialize();
    }
}
