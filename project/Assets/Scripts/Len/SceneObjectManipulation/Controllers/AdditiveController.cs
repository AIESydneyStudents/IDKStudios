using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditiveController : InteractionController
{
    public AdditiveInterface additiveInterface;


    public AdditiveController()
    {
        controllerType = ControllerType.ADDITIVE;
    }

    public override void Interact()
    {
        InteractHightlight();
        EnableCollisionTrigger();
        FindValidControllers();
        HighlightValidControllers();

        InputController.Instance.HoldObject(gameObject);
    }

    public override void Uninteract()
    {
        if (compatibleController != null)
        {
            ContainerController compatibleContainerController =
                (ContainerController)compatibleController;

            switch (compatibleContainerController.associatedInterface.interfaceType)
            {
                case Interface.InterfaceType.TEAPOT_INTERFACE:
                    {
                        TeapotInterface teapotInterface =
                            (TeapotInterface)compatibleContainerController.associatedInterface;

                        teapotInterface.teapot.InsertAdditive(additiveInterface.containedAdditive);

                        break;
                    }
                case Interface.InterfaceType.CUP_INTERFACE:
                    {
                        CupInterface cupInterface =
                            (CupInterface)compatibleContainerController.associatedInterface;

                        cupInterface.cup.InsertAdditive(additiveInterface.containedAdditive);

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        InputController.Instance.UnholdObject();
        UnhighlightValidControllers();
        Destroy(gameObject);
    }
}
