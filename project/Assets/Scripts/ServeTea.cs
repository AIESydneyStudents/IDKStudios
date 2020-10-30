using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServeTea : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Saucer")
        {
            GameObject Tea = collider.gameObject;

            // WILL ADD ANIMATION HERE

            Destroy(Tea, 0.5f);
        }
    }
}
