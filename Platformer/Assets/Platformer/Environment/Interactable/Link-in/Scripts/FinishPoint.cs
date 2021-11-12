using UnityEngine;
using Zenject;

public class FinishPoint : MonoBehaviour
{
    [Inject] private RepositoryBase repositoryBase;

    [SerializeField] private AudioClip winSFX;
    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();       
    }
    private void Start()
    {
        audioSource.volume = repositoryBase.playerSettingsObj.characterVolume;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)//player
        {
            audioSource.PlayOneShot(winSFX);
            EventManager.CallOnLevelComplete();
        }
    }
}
