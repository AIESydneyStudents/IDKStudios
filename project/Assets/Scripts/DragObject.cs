using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using CustomEditor;

public class DragObject : Singleton<DragObject>
{
    private Vector3 originalObjPos;
    private Vector3 offset;

    [HideInInspector]
    public bool snappedToStation;
    [HideInInspector]
    public bool onWorkStation;
    [SerializeField]
    public bool needsAnchor;

    [ConditionalField("needsAnchor", false)]
    public GameObject startAnchor;
    private GameObject teleportAnchor;
    private GameObject targetObject;
    private GameObject previousObject;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startAnchor.transform.position;
        originalObjPos = transform.position;
        onWorkStation = false;
        snappedToStation = false;
    }

    // Returns mouse coordinates
    private Vector3 GetMouseWorldPos()
    {
        // Raycast between certain layers
        int layerMask = 1 << 9;
        layerMask = ~layerMask;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // If raycast recieves a hit
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            return hit.point;
        }

        else
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    // Used to preview object on a nearby workstation
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

    // When mouse is released
    void OnMouseUp()
    {
        // Raycast between certain layers
        int layerMask = 1 << 8;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // If raycast receives a hit
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            // Item's destination
            targetObject = hit.transform.gameObject;

            // If workstation is not occupied 
            if (targetObject.GetComponent<WorkStationEvent>().inUse == false)
            {
                // Sets item's anchor to anchor attchacted to workstation
                teleportAnchor = hit.transform.GetChild(0).gameObject;

                // Sets item's position to anchor
                transform.position = teleportAnchor.transform.position;

                // Sets the workstation teleported to as occupied 
                targetObject.GetComponent<WorkStationEvent>().inUse = true;

                // If item had a previous workstation it is not longer in use
                if (previousObject != null)
                {
                    previousObject.GetComponent<WorkStationEvent>().inUse = false;
                }

                // Sets current workstation as previous workstation
                previousObject = targetObject;
            }
        }

        // If raycast does not receive a hit
        else
        {
            if (needsAnchor)
            {
                // Sets object's position to it's starting position
                transform.position = originalObjPos;
            }

            // Current Workstation is not longer occupied
            targetObject.GetComponent<WorkStationEvent>().inUse = false;

            // If item had a previous workstation it is not longer in use
            if (previousObject != null)
            {
                previousObject.GetComponent<WorkStationEvent>().inUse = false;
            }

            previousObject = null;
        }
    }

    // When mouse is down
    void OnMouseDown()
    {
        offset = new Vector3(0, 0.05f, 0.1f);
    }

    // When mouse is being dragged across screen
    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + offset;

        // Calls function to preview object on workstation
        Preview();
    }

    // CAN POSSIBLY GET RID OF
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "WorkStation")
        {
            onWorkStation = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "WorkStation")
        {
            onWorkStation = false;
        }
    }
}