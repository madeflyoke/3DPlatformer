using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractController : MonoBehaviour
{
    public GameObject interactObject;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactObject.GetComponent<IObstacle>().locking = false;
            Destroy(gameObject);
        }
    }
}
