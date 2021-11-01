using UnityEngine;

public class InteractItemDrop : MonoBehaviour
{
    [SerializeField] private GameObject item;
    [SerializeField] private GameObject dropEffect;
    [SerializeField] private GameObject objectToInteract;

    private void OnDestroy()
    {       
        GameObject go = Instantiate(item, transform.position+(Vector3.up/2), Quaternion.identity);
        Instantiate(dropEffect, go.transform, false);
        go.GetComponent<InteractController>().interactObject = objectToInteract;
        

    }
}
