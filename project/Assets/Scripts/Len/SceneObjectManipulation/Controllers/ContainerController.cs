﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerController : InteractionController
{
    public Interface associatedInterface;
    public AudioClip audioClip;

    private void Update()
    {
        UpdateProgressBar();
    }

    public override void Interact()
    {
        switch (associatedInterface.interfaceType)
        {
            case Interface.InterfaceType.TAP_INTERFACE:
                {
                    break;
                }
            case Interface.InterfaceType.KETTLE_INTERFACE:
                {
                    InteractHightlight();
                    EnableCollisionTrigger();
                    FindValidControllers();
                    HighlightValidControllers();

                    InputController.Instance.HoldObject(gameObject);
                    LeaveAnchor();
                    HideProgressBar();
                    ((KettleInterface)associatedInterface).kettle.IsActive = false;

                    break;
                }
            default:
                {
                    InteractHightlight();
                    EnableCollisionTrigger();
                    FindValidControllers();
                    HighlightValidControllers();

                    InputController.Instance.HoldObject(gameObject);
                    LeaveAnchor();

                    break;
                }
        }
    }

    public override void Uninteract()
    {
        if (compatibleController != null)
        {
            ContainerController compatibleContainerController =
                (ContainerController)compatibleController;

            switch (compatibleContainerController.associatedInterface.interfaceType)
            {
                case Interface.InterfaceType.TAP_INTERFACE:
                    {
                        switch (associatedInterface.interfaceType)
                        {
                            case Interface.InterfaceType.KETTLE_INTERFACE:
                                {
                                    Vector3 screenCoordinates = Input.mousePosition;
                                    InputController.Instance.HideInformationReadout();
                                    menuController.ShowMenu(screenCoordinates);

                                    break;
                                }
                            case Interface.InterfaceType.TEAPOT_INTERFACE:
                                {
                                    AudioSource.PlayClipAtPoint(audioClip, Vector3.zero);

                                    TeapotInterface teapotInterface =
                                        (TeapotInterface)associatedInterface;

                                    teapotInterface.teapot.ResetTeapot();
                                    GameEventManager.Instance.docketUI.TriggerIconUpdate();
                                    ReturnToAnchor();

                                    break;
                                }
                            case Interface.InterfaceType.CUP_INTERFACE:
                                {
                                    AudioSource.PlayClipAtPoint(audioClip, Vector3.zero);

                                    CupInterface cupInterface =
                                        (CupInterface)associatedInterface;

                                    cupInterface.cup.ResetCup();
                                    GameEventManager.Instance.docketUI.TriggerIconUpdate();
                                    ReturnToAnchor();

                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }

                        break;
                    }
                case Interface.InterfaceType.TEAPOT_INTERFACE:
                    {
                        TeapotInterface teapotInterface =
                            (TeapotInterface)compatibleContainerController.associatedInterface;

                        KettleInterface kettleInterface =
                            (KettleInterface)associatedInterface;

                        kettleInterface.kettle.DispenseToTeapot(teapotInterface.teapot);
                        ReturnToAnchor();

                        break;
                    }
                case Interface.InterfaceType.CUP_INTERFACE:
                    {
                        CupInterface cupInterface =
                            (CupInterface)compatibleContainerController.associatedInterface;

                        TeapotInterface teapotInterface =
                            (TeapotInterface)associatedInterface;

                        
                        teapotInterface.teapot.DispenseToCup(cupInterface.cup);
                        GameEventManager.Instance.docketUI.TriggerIconUpdate();
                        ReturnToAnchor();

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        else
        {
            ReturnToAnchor();
        }

        InputController.Instance.UnholdObject();
        UnhighlightValidControllers();
        ClearValidControllers();
    }

    public override void ShowProgressBar()
    {
        switch (associatedInterface.interfaceType)
        {
            case Interface.InterfaceType.KETTLE_INTERFACE:
                {
                    KettleInterface kettleInterface =
                        (KettleInterface)associatedInterface;

                    progressBarController.SetStartEnd(
                        kettleInterface.kettle.Temperature,
                        kettleInterface.kettle.TemperatureSetting);

                    Vector3 screenCoordinates = Input.mousePosition;
                    progressBarController.SetPosition(screenCoordinates + progressBarOffset);
                    progressBarController.ShowProgressBar();

                    return;
                }
            default:
                {
                    return;
                }
        }
    }

    public override void UpdateProgressBar()
    {
        switch (associatedInterface.interfaceType)
        {
            case Interface.InterfaceType.KETTLE_INTERFACE:
                {
                    KettleInterface kettleInterface =
                        (KettleInterface)associatedInterface;

                    progressBarController.SetProgress(kettleInterface.kettle.Temperature);

                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}
