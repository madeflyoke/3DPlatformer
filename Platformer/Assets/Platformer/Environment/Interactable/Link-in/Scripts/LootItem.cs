using UnityEngine;

public class LootItem : InteractObject
{ 
    public GameObject interactObj { get=>interactObject; set => interactObject = value; }

    protected override void ObjectDisable()
    {
        AudioManager.instance.PlayClip(unlockSFX, AudioManager.instance.envVolume);
        Destroy(gameObject);
    }
}
