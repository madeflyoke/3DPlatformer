using UnityEngine;

public class PlayerSettings 
{
    private const string CharacterVolumeKey = "CharacterVolume";
    private const string EnemyVolumeKey = "EnemyVolume";
    private const string EnvVolumeKey = "EnvVolume";
    private const string MusicVolumeKey = "MusicVolume";
    
    public float characterVolume { get; private set; } = 0.5f;
    public float enemyVolume { get; private set; } = 0.4f;
    public float envVolume { get; private set; } = 0.1f;
    public float musicVolume { get; private set; } = 0.4f;

    public void Initialize()
    {
        if (!PlayerPrefs.HasKey(CharacterVolumeKey))
        {
            PlayerPrefs.SetFloat(CharacterVolumeKey, characterVolume);
            PlayerPrefs.SetFloat(EnemyVolumeKey, enemyVolume);
            PlayerPrefs.SetFloat(EnvVolumeKey, envVolume);
            PlayerPrefs.SetFloat(MusicVolumeKey, musicVolume);
        }
        else
        {
            characterVolume = PlayerPrefs.GetFloat(CharacterVolumeKey);
            enemyVolume = PlayerPrefs.GetFloat(EnemyVolumeKey);
            envVolume = PlayerPrefs.GetFloat(EnvVolumeKey);
            musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey);
        }
    }
    private void ResetAudioSettings()
    {
        PlayerPrefs.DeleteKey(CharacterVolumeKey);
        PlayerPrefs.DeleteKey(EnemyVolumeKey);
        PlayerPrefs.DeleteKey(EnvVolumeKey);
        PlayerPrefs.DeleteKey(MusicVolumeKey);

    }

}
