using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditiveSourceController : InteractionController
{
    public GameObject additiveObjectToSpawn;
    public Additive containedAdditive;

    public AdditiveSourceController()
    {
        controllerType = ControllerType.ADDITIVE_SOURCE;
    }

    public override void Interact()
    {
        if (additiveObjectToSpawn == null)
        {
            return;
        }

        GameObject newAdditiveObject = Instantiate(additiveObjectToSpawn);
        newAdditiveObject.GetComponent<AdditiveController>().Interact();
    }
}
