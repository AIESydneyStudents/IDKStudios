using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStationEvent : Singleton<WorkStationEvent>
{
    private bool inUse;

    // Start is called before the first frame update
    void Start()
    {
        inUse = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if ()
        //{

        //}
    }

    private void IsStationInUse()
    {
        if (DragObject.instance.onWorkStation == true)
        {

        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Item")
        {
            inUse = true;
        }
    }
}
