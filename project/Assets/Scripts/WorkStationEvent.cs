using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStationEvent : Singleton<WorkStationEvent>
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
        //if (DragObject.instance.onWorkStation == true)
        //{
        //    Destroy(gameObject);
        //}

        //else if (DragObject.instance.onWorkStation == false)
        //{
        //    Destroy(gameObject);
        //}
    }
}