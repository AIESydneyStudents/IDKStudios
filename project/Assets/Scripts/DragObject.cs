using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DragObject : Singleton<DragObject>
{
    private Vector3 originalObjPos;
    private Vector3 workStationAnchor;
    private Vector3 offset;
    private float delay;
    private float timer;
    [HideInInspector]
    public bool onWorkStation;

    [SerializeField]
    public GameObject StartAnchor;
    private GameObject TeleportAnchor;

    private Rigidbody rb;

    void Start()
    {
        transform.position = StartAnchor.transform.position;
        originalObjPos = transform.position;
        onWorkStation = false;

        delay = 4;

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Increase the value of 'timer' by deltaTime:
        timer += Time.deltaTime;

        Debug.Log(onWorkStation);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("SampleScene");
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

    private void CookingTime()
    {
        if (timer > delay)
        {
            //If so, proceed to translate the object:
            transform.position = originalObjPos;
        }
    }

    void OnMouseUp()
    {
        int layerMask = 1 << 8;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            TeleportAnchor = hit.transform.GetChild(0).gameObject;
            workStationAnchor = TeleportAnchor.transform.position;

            onWorkStation = true;

            Debug.Log("Snapped to Work Station");
            transform.position = workStationAnchor;
        }
    }

    void OnMouseDown()
    {
        offset = new Vector3(0, 0.1f, -0.1f);
    }

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
}