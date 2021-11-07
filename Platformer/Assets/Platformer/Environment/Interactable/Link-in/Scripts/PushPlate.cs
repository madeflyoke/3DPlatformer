using UnityEngine;

public class PushPlate : InteractObject
{
    [SerializeField] private Renderer buttonRenderer;  
    protected override void ObjectDisable()
    {
        buttonRenderer.material.color = Color.cyan;
        audioManager.PlayClip(unlockSFX, repositoryBase.playerSettingsObj.envVolume);
        isEnabled = false;
    }

}
