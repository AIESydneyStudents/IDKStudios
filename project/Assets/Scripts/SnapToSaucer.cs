using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToSaucer : MonoBehaviour
{
    private void Preview()
    {
        // Raycast between certain layers
        int layerMask = 1 << 9;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // If raycast recieves a hit
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            transform.position = hit.transform.GetChild(0).gameObject.transform.position;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Saucer")
        {
            Preview();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Saucer")
        {
            Preview();
        }
    }
}
