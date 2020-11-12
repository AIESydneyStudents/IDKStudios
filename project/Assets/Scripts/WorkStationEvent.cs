using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStationEvent : MonoBehaviour
{
    [HideInInspector]
    public bool inUse;

    // Start is called before the first frame update
    void Start()
    {
        inUse = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Enables/Disables colliders if occupied or not
        if (inUse)
        {
            gameObject.GetComponent<SphereCollider>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }

        else if (!inUse)
        {
            gameObject.GetComponent<SphereCollider>().enabled = true;
            gameObject.GetComponent<BoxCollider>().enabled = true;
        }
    }
}