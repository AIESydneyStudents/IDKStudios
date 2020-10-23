using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    private Vector3 offset;

    [SerializeField]
    public GameObject objToSpawn;
    private GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

    void OnMouseDown()
    {
        //offset = new Vector3(0, 0.1f, -0.1f);

        // Spawning Object
        obj = GameObject.Instantiate(objToSpawn);
    }

    // When mouse is being dragged across screen
    void OnMouseDrag()
    {
        obj.transform.position = GetMouseWorldPos() /*+ offset*/;
    }
}
