using UnityEngine;
using Zenject;

public class InteractItemDrop : MonoBehaviour
{
    [Inject] private DiContainer diContainer;

    [SerializeField] private GameObject item;
    [SerializeField] private GameObject dropEffect;
    [SerializeField] private GameObject objectToInteract;
    private GameObject itemObj;
    
    private void Awake()
    {
        GetComponent<Enemy>().haveLootItem = true;
        item.GetComponent<LootItem>().interactObj = objectToInteract;
        itemObj = diContainer.InstantiatePrefab(item, transform.position + (Vector3.up / 2), Quaternion.identity, null);
        Instantiate(dropEffect, itemObj.transform, false);
        itemObj.SetActive(false);
    }

    public void DropItem()
    {
        itemObj.SetActive(true);
    }

}
