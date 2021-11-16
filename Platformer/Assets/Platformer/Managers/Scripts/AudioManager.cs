using UnityEngine;
using Zenject;
using System.Collections;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    [Inject] private RepositoryBase repositoryBase;
    [SerializeField] private AudioClip battleTheme;
    [SerializeField] private AudioClip mainMenuTheme;
    [SerializeField] private AudioClip evilLevelTheme;
    [SerializeField] private AudioClip winSFX;
    [SerializeField] private AudioClip loseSFX;

    private AudioSource[] audioSources; //audio channels
    private AudioClip currentMainTheme;
    private Coroutine delayBeforeMainTheme;
    private Tween audioTween;
    private void Awake()
    {
        audioSources = GetComponents<AudioSource>();
        audioSources[0].loop = true;
        audioSources[0].volume = repositoryBase.playerSettingsObj.musicVolume;
    }

    private void OnEnable()
    {
        GameStateController.setSceneEvent += SetMainTheme;
        GameStateController.setGameStateEvent += ByGameState;
        PlayerController.playerCurrentStateEvent += ByPlayerState;
    }

    private void OnDisable()
    {
        GameStateController.setSceneEvent -= SetMainTheme;
        GameStateController.setGameStateEvent -= ByGameState;
        PlayerController.playerCurrentStateEvent -= ByPlayerState;
    }

    private void FadeTransitionAudio(AudioClip toClip, float fromSpeed, float toSpeed)
    {
        audioTween = audioSources[0].DOFade(0f, fromSpeed).OnComplete(() =>
        {
            audioSources[0].Stop();
            audioSources[0].clip = toClip;
            audioSources[0].PlayDelayed(1f);
            audioSources[0].volume = repositoryBase.playerSettingsObj.musicVolume;           
        });

    }

    private IEnumerator DelayBeforeMainTheme()
    {
        if (audioTween != null && audioTween.IsPlaying())
            audioTween.Kill();                
        yield return new WaitForSeconds(3f);
        FadeTransitionAudio(currentMainTheme, 3f, 0.5f);
        yield return new WaitWhile(audioTween.IsActive);
        delayBeforeMainTheme = null;
        audioTween = null;
    }

    private void ByPlayerState(PlayerState state)
    {
        if (state == PlayerState.Attack)
        {
            if (delayBeforeMainTheme != null)
            {
                StopCoroutine(delayBeforeMainTheme);
                delayBeforeMainTheme = null;
            }
            if (audioTween != null && audioTween.IsPlaying()) 
            {
                audioTween.Kill();
                audioTween = null;
            }               
            if (audioSources[0].clip!=battleTheme)
            {
                audioSources[0].Stop();
                audioSources[0].volume = repositoryBase.playerSettingsObj.musicVolume;
                audioSources[0].clip = battleTheme;
                audioSources[0].Play();
            }         
        }
        else if (audioSources[0].clip != currentMainTheme && state != PlayerState.Attack && delayBeforeMainTheme == null)
        {
            delayBeforeMainTheme = StartCoroutine(DelayBeforeMainTheme());                   
        }
        if (state == PlayerState.Died)
        {
            audioSources[0].Stop();
        }

    }

    private void ByGameState(GameState state)
    {
        switch (state)
        {
            case GameState.Menu:
                currentMainTheme = mainMenuTheme;
                audioSources[0].clip = currentMainTheme;
                audioSources[0].Play();
                break;
            case GameState.LevelComplete:
                audioSources[0].Stop();
                audioSources[0].PlayOneShot(winSFX);
                break;
            case GameState.EndGame:
                audioSources[0].Stop();
                audioSources[0].PlayOneShot(loseSFX);
                break;
        }
    }

    private void SetMainTheme(int sceneIndex)
    {
        switch (sceneIndex)
        {
            case 1:
                currentMainTheme = evilLevelTheme;
                audioSources[0].Stop();
                audioSources[0].clip = currentMainTheme;
                break;
        }
        audioSources[0].PlayDelayed(2f);
    }

    public void PlayClip(AudioClip audioClip, float volume)
    {
        audioSources[1].PlayOneShot(audioClip, volume);
    }

}
