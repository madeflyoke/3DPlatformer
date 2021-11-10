using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class AudioManager : MonoBehaviour
{
    [Inject] private RepositoryBase repositoryBase;
    [SerializeField] private AudioClip evilLevelTheme;
    [SerializeField] private AudioClip someNextLevelTheme;

    private AudioSource[] audioSources; //audio channels

    private void Awake()
    {
        audioSources = GetComponents<AudioSource>();
        audioSources[0].loop = true;
        audioSources[0].volume = repositoryBase.playerSettingsObj.musicVolume;       
    }
    private void OnEnable()
    {
        EventManager.setSceneEvent += SetMainTheme;
        EventManager.levelCompleteEvent += () => audioSources[0].Stop();
    }

    private void OnDisable()
    {
        EventManager.setSceneEvent += SetMainTheme;
        EventManager.levelCompleteEvent -= () => audioSources[0].Stop();
    }
    private void SetMainTheme(int sceneIndex)
    {
        switch (sceneIndex)
        {
            case 1:
                audioSources[0].clip = evilLevelTheme;
                break;
        }     
        audioSources[0].PlayDelayed(2f);
    }

    public void PlayClip(AudioClip audioClip, float volume)
    {
        audioSources[1].PlayOneShot(audioClip, volume);
    }




}
