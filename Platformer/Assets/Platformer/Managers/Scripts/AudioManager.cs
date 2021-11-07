using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioClip evilLevelTheme;
    [SerializeField] private AudioClip someNextLevelTheme;

    private AudioSource[] audioSources;

    public readonly float playerVolume = 0.5f;
    public readonly float enemyVolume = 0.4f;
    public readonly float envVolume = 0.2f;
    public readonly float musicVolume = 0.4f;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        audioSources = GetComponents<AudioSource>();
        audioSources[0].loop = true;
        audioSources[0].clip = evilLevelTheme;
        audioSources[0].volume = musicVolume;
        audioSources[0].PlayDelayed(2f);
    }

    private void OnEnable()
    {
        EventManager.setSceneEvent += SetMainTheme;
    }

    private void OnDisable()
    {
        EventManager.setSceneEvent -= SetMainTheme;
    }

    private void SetMainTheme(int sceneIndex)
    {
        switch (sceneIndex)
        {
            case 0:
                audioSources[0].clip = evilLevelTheme;
                break;
            case 1:
                Debug.Log("nextThemePlaying...");
                break;
        }
        audioSources[0].PlayDelayed(3f);
    }

    public void PlayClip(AudioClip audioClip, float volume)
    {
        audioSources[1].PlayOneShot(audioClip, volume);
    }




}
