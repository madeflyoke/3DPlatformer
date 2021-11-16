using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerObstacle : Obstacle
{
    [SerializeField] private AudioClip flameSFX;

    private void Start()
    {
        audioSource.loop = true;
        audioSource.clip = flameSFX;
        audioSource.Play();
    }

    public override void Unlock()
    {
        obstacle.enabled = false;
        gameObject.SetActive(false);
        audioSource.Stop();
    }
}
