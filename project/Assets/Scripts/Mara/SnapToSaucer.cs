using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToSaucer : MonoBehaviour
{
    private bool onSaucer = false;

    [SerializeField]
    public GameObject servingStation;
    private GameObject teleportAnchor;
    private GameObject saucer;

    private void Preview()
    {
        // Raycast between certain layers
        int layerMask = 1 << 8;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // If raycast recieves a hit
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            transform.position = hit.transform.GetChild(0).gameObject.transform.position;
        }
    }

    private void OnMouseUp()
    {
        if (onSaucer)
        {
            transform.parent = saucer.transform;

            servingStation.GetComponent<WorkStationEvent>().inUse = true;

            teleportAnchor = servingStation.transform.GetChild(0).gameObject;

            saucer.transform.position = teleportAnchor.transform.position;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Saucer")
        {
            Preview();

            saucer = collider.gameObject;

            onSaucer = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Saucer")
        {
            Preview();

            saucer = null;

            onSaucer = false;
        }
    }
}
