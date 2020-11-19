using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorController : InteractionController
{
    public GameObject anchor;
    public InteractionController visitingController;

    public override void Interact()
    {
        switch (menuController.menuType)
        {
            case MenuController.MenuType.KETTLE_MENU:
                {
                    if (visitingController == null)
                    {
                        return;
                    }

                    KettleMenuController kettleMenuController =
                        (KettleMenuController)menuController;

                    Vector3 screenCoordinates = Input.mousePosition;
                    kettleMenuController.ShowMenu(screenCoordinates);

                    break;
                }
            case MenuController.MenuType.SAUCER_MENU:
                {
                    if (visitingController == null)
                    {
                        return;
                    }

                    SaucerMenuController saucerMenuController =
                        (SaucerMenuController)menuController;

                    Vector3 screenCoordinates = Input.mousePosition;
                    saucerMenuController.ShowMenu(screenCoordinates);

                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    public Transform GetAnchorTransform()
    {
        return anchor.transform;
    }
}
