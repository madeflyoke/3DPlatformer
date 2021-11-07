using UnityEngine;


public class FinishPoint : MonoBehaviour
{
    [SerializeField] private AudioClip winSFX;
    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();       
    }
    private void Start()
    {
        audioSource.volume = AudioManager.instance.playerVolume;
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
