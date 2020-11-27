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
                        GameEventManager.Instance.teapotUI.UpdateIcons();
                        GameEventManager.Instance.PushTeaStage(GameEventManager.TeaStage.FILL_CUP);

                        break;
                    }
                case Interface.InterfaceType.CUP_INTERFACE:
                    {
                        CupInterface cupInterface =
                            (CupInterface)compatibleContainerController.associatedInterface;

                        cupInterface.cup.InsertAdditive(additiveInterface.containedAdditive);
                        GameEventManager.Instance.cupUI.UpdateIcons();
                        
                        if (additiveInterface.containedAdditive.additiveType == Additive.Type.CONDIMENT)
                        {
                            GameEventManager.Instance.PushTeaStage(GameEventManager.TeaStage.ADD_MILK);
                        }

                        if (additiveInterface.containedAdditive.additiveType == Additive.Type.MILK)
                        {
                            GameEventManager.Instance.PushTeaStage(GameEventManager.TeaStage.SERVE);
                        }

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
