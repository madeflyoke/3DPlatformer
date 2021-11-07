using UnityEngine;

public class LootItem : InteractObject
{
    public GameObject interactObj { get=>interactObject; set => interactObject = value; }
    protected override void ObjectDisable()
    {
        audioManager.PlayClip(unlockSFX, repositoryBase.playerSettingsObj.envVolume);
        Destroy(gameObject);
    }
}
