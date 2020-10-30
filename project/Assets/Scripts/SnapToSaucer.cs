using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToSaucer : MonoBehaviour
{
    private bool onSaucer = false;

    [SerializeField]
    public GameObject ServingStation;
    private GameObject TeleportAnchor;
    private GameObject Saucer;

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
            transform.parent = Saucer.transform;

            ServingStation.GetComponent<WorkStationEvent>().inUse = true;

            TeleportAnchor = ServingStation.transform.GetChild(0).gameObject;

            Saucer.transform.position = TeleportAnchor.transform.position;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Saucer")
        {
            Preview();

            Saucer = collider.gameObject;

            onSaucer = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Saucer")
        {
            Preview();

            Saucer = null;

            onSaucer = false;
        }
    }
}
