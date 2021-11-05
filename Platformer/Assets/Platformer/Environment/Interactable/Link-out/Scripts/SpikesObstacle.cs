using DG.Tweening;
using UnityEngine;

public class SpikesObstacle : Obstacle
{
    [SerializeField] private float YScaleToHide = 0f;
    public override void Unlock()
    {      
        transform.DOScaleY(YScaleToHide, animationUnlockTime);
        obstacle.enabled = false;
        audioSource.PlayOneShot(unlockSFX);
    }
}
