using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDespawn : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Item")
        {
            Destroy(collider.gameObject, 0.5f);
        }
    }
}
