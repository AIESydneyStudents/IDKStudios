using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditiveSourceWithMenuController : InteractionController
{
    public GameObject additiveObjectToSpawn1;
    public GameObject additiveObjectToSpawn2;
    public Additive containedAdditive1;
    public Additive containedAdditive2;

    public AdditiveSourceWithMenuController()
    {
        controllerType = ControllerType.ADDITIVE_SOURCE_WITH_MENU;
    }

    public override void Interact()
    {
        Vector3 screenCoordinates = Input.mousePosition;
        menuController.ShowMenu(screenCoordinates);
    }

    public void SpawnObject1()
    {
        if (additiveObjectToSpawn1 == null)
        { 
            return;
        }

        GameObject newAdditiveObject = Instantiate(additiveObjectToSpawn1);
        newAdditiveObject.GetComponent<AdditiveController>().Interact();
    }

    public void SpawnObject2()
    {
        if (additiveObjectToSpawn2 == null)
        {
            return;
        }

        GameObject newAdditiveObject = Instantiate(additiveObjectToSpawn2);
        newAdditiveObject.GetComponent<AdditiveController>().Interact();
    }
}
