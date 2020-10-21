using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DragObject : Singleton<DragObject>
{
    private Vector3 originalObjPos;
    private Vector3 workStationAnchor;
    private Vector3 offset;

    [HideInInspector]
    public bool snappedToStation;
    [HideInInspector]
    public bool onWorkStation;

    [SerializeField]
    public GameObject StartAnchor;
    private GameObject TeleportAnchor;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = StartAnchor.transform.position;
        originalObjPos = transform.position;
        onWorkStation = false;
        snappedToStation = false;

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(onWorkStation);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("DragDropTest");
        }
    }

    // Returns mouse coordinates
    private Vector3 GetMouseWorldPos()
    {
        int layerMask = 1 << 9;
        layerMask = ~layerMask;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            return hit.point;
        }

        else
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    // When mouse is released
    void OnMouseUp()
    {
        int layerMask = 1 << 8;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            TeleportAnchor = hit.transform.GetChild(0).gameObject;
            workStationAnchor = TeleportAnchor.transform.position;

            Debug.Log("Snapped to Work Station");
            transform.position = workStationAnchor;

            // THIS WILL BE REMOVED, PUT IN FOR TESTING PURPOSES 
            //Destroy(TeleportAnchor);

            snappedToStation = true;
        }

        else
        {
            transform.position = originalObjPos;

            snappedToStation = false;
        }
    }

    // When mouse is down
    void OnMouseDown()
    {
        offset = new Vector3(0, 0.1f, -0.1f);
    }

    // When mouse is being dragged across screen
    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + offset;
    }

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