using UnityEngine;
using Zenject;

public class AudioManager : MonoBehaviour
{
    [Inject] private RepositoryBase repositoryBase;
    [SerializeField] private AudioClip evilLevelTheme;
    [SerializeField] private AudioClip someNextLevelTheme;

    private AudioSource[] audioSources;

    private void Awake()
    {
        audioSources = GetComponents<AudioSource>();
        audioSources[0].loop = true;
        audioSources[0].volume = repositoryBase.playerSettingsObj.musicVolume;       
    }
    private void Start()
    {
        SetMainTheme(0);
    }
    private void OnEnable()
    {
        EventManager.changeSceneEvent += SetMainTheme;
        EventManager.levelCompleteEvent += () => audioSources[0].Stop();
    }

    private void OnDisable()
    {
        EventManager.changeSceneEvent -= SetMainTheme;
        EventManager.levelCompleteEvent -= () => audioSources[0].Stop();

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
        audioSources[0].PlayDelayed(2f);
    }

    public void PlayClip(AudioClip audioClip, float volume)
    {
        audioSources[1].PlayOneShot(audioClip, volume);
    }




}
