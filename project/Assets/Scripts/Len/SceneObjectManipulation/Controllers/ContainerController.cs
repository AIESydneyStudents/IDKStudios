using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerController : InteractionController
{
    public Interface associatedInterface;
    public AudioClip audioClip1;
    public AudioClip audioClip2;

    private void Update()
    {
    }

    public override void Interact()
    {
        InputController.Instance.HideInformationReadout();

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
                    GameEventManager.Instance.kettleUI.gameObject.SetActive(false);

                    KettleInterface kettleInterface = (KettleInterface)associatedInterface;

                    if (kettleInterface.kettle.IsActive)
                    {
                        kettleInterface.kettle.audioSource.Stop();
                        kettleInterface.kettle.audioSource.clip = kettleInterface.kettle.heatupDone;
                        kettleInterface.kettle.audioSource.Play();
                    }

                    kettleInterface.kettle.IsActive = false;

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
                                    KettleInterface kettleInterface =
                                        (KettleInterface)associatedInterface;
                                    
                                    if (kettleInterface.kettle.IsFull)
                                    {
                                        //Emptying sound.
                                        AudioSource.PlayClipAtPoint(audioClip1, Vector3.zero);
                                        kettleInterface.kettle.ResetKettle();
                                        ReturnToAnchor();
                                        GameEventManager.Instance.kettleUI.gameObject.SetActive(true);
                                    }
                                    else
                                    {
                                        //Filling sound.
                                        AudioSource.PlayClipAtPoint(audioClip2, Vector3.zero);
                                        kettleInterface.kettle.FillFromTap();
                                        ReturnToAnchor();
                                        GameEventManager.Instance.kettleTemperatureUI.ShowMenu(InputController.Instance.camera.WorldToScreenPoint(kettleInterface.gameObject.transform.position));
                                        InputController.Instance.DisableInteraction();

                                        GameEventManager.Instance.PushTeaStage(GameEventManager.TeaStage.FILL_TEAPOT);
                                    }

                                    GameEventManager.Instance.kettleUI.UpdateIcons();

                                    break;
                                }
                            case Interface.InterfaceType.TEAPOT_INTERFACE:
                                {
                                    AudioSource.PlayClipAtPoint(audioClip1, Vector3.zero);

                                    TeapotInterface teapotInterface =
                                        (TeapotInterface)associatedInterface;

                                    teapotInterface.teapot.ResetTeapot();
                                    ReturnToAnchor();

                                    GameEventManager.Instance.teapotUI.UpdateIcons();

                                    break;
                                }
                            case Interface.InterfaceType.CUP_INTERFACE:
                                {
                                    AudioSource.PlayClipAtPoint(audioClip1, Vector3.zero);

                                    CupInterface cupInterface =
                                        (CupInterface)associatedInterface;

                                    cupInterface.cup.ResetCup();
                                    ReturnToAnchor();

                                    GameEventManager.Instance.cupUI.UpdateIcons();

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

                        AudioSource.PlayClipAtPoint(audioClip1, Vector3.zero);
                        kettleInterface.kettle.DispenseToTeapot(teapotInterface.teapot);
                        ReturnToAnchor();

                        GameEventManager.Instance.kettleUI.UpdateIcons();
                        GameEventManager.Instance.teapotUI.UpdateIcons();

                        GameEventManager.Instance.PushTeaStage(GameEventManager.TeaStage.ADD_TEA);

                        

                        break;
                    }
                case Interface.InterfaceType.CUP_INTERFACE:
                    {
                        CupInterface cupInterface =
                            (CupInterface)compatibleContainerController.associatedInterface;

                        TeapotInterface teapotInterface =
                            (TeapotInterface)associatedInterface;

                        AudioSource.PlayClipAtPoint(audioClip1, Vector3.zero);
                        teapotInterface.teapot.DispenseToCup(cupInterface.cup);
                        ReturnToAnchor();

                        GameEventManager.Instance.teapotUI.UpdateIcons();
                        GameEventManager.Instance.cupUI.UpdateIcons();

                        GameEventManager.Instance.PushTeaStage(GameEventManager.TeaStage.ADD_CONDIMENT);


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

            switch (associatedInterface.interfaceType)
            {
                case Interface.InterfaceType.KETTLE_INTERFACE:
                    {
                        GameEventManager.Instance.kettleUI.gameObject.SetActive(true);

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
        ClearValidControllers();
    }
}
