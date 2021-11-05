using UnityEngine;
using System.Collections;

public class InteractItemDrop : MonoBehaviour
{
    [SerializeField] private GameObject item;
    [SerializeField] private GameObject dropEffect;
    [SerializeField] private GameObject objectToInteract;
    private GameObject itemObj;

    private void Awake()
    {
        GetComponent<Enemy>().haveLootItem = true;
        item.GetComponent<LootItem>().interactObj = objectToInteract;
        itemObj = Instantiate(item, transform.position + (Vector3.up / 2), Quaternion.identity);
        Instantiate(dropEffect, itemObj.transform, false);
        itemObj.SetActive(false);
    }

    public void DropItem()
    {
        itemObj.SetActive(true);
    }

}
