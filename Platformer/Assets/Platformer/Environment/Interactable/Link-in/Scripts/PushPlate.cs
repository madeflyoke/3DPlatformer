using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPlate : InteractObject
{
    [SerializeField] private Renderer buttonRenderer;  
    protected override void ObjectDisable()
    {
        buttonRenderer.material.color = Color.cyan;
        AudioManager.instance.PlayClip(unlockSFX, AudioManager.instance.envVolume);
        enabled = false;
    }

}
