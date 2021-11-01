using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPlate : MonoBehaviour
{
    [SerializeField] private GameObject interactObject;
    [SerializeField] private Material buttonMaterial;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            buttonMaterial.color = Color.blue;
            interactObject.GetComponent<IObstacle>().locking = false;
            enabled = false;
        }
    }

}
