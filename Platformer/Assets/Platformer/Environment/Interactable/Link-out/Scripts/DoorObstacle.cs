using UnityEngine;
using DG.Tweening;
using System.Collections;

public class DoorObstacle : Obstacle
{
    [SerializeField] private GameObject pivot;
    public override void Unlock()
    {
        isLocked = false;
    }
    private void OnTriggerEnter(Collider other)
    {     
        if (!isLocked)
        {
            if (other.gameObject.layer == 3)
            {
                pivot.transform.DOLocalRotate(new Vector3(0f, 90f, 0f), 1f);
                obstacle.enabled = false;
                audioSource.PlayOneShot(unlockSFX);
                isLocked = true;
            }
        }
    }


}
