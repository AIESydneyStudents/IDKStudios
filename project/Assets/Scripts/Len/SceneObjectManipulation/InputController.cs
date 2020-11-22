using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : Singleton<InputController>
{
    public Camera camera;

    public LayerMask dropLayer;
    public LayerMask dragLayer;
    public string interactionTag;

    public Ray cameraRay;
    public RaycastHit rayHit;
    public RaycastHit[] rayHits;

    public MeshCollider dragPlane;

    public GameObject pointingAtGameObject;
    public InteractionController pointingAtController;

    public bool enabled;
    public bool isHoldingItem;

    public AdditiveReadoutController additiveReadoutController;
    public ContainerReadoutController containerReadoutController;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameEventManager.Instance.TogglePauseGame();
        }

        if (!enabled)
        {
            return;
        }

        cameraRay = camera.ScreenPointToRay(Input.mousePosition);

        if (!isHoldingItem)
        {
            if (Physics.Raycast(cameraRay, out rayHit, 100, dropLayer) &&
                rayHit.collider.CompareTag(interactionTag))
            {
                if (pointingAtGameObject != rayHit.collider.gameObject)
                {
                    if (pointingAtGameObject != null)
                    {
                        pointingAtController.Unhighlight();
                    }

                    pointingAtGameObject = rayHit.collider.gameObject;
                    pointingAtController = pointingAtGameObject.GetComponent<InteractionController>();
                    pointingAtController.HoverHighlight();
                    ShowInformationReadout();
                }
            }
            else
            {
                if (pointingAtGameObject != null)
                {
                    pointingAtController.Unhighlight();
                }

                pointingAtGameObject = null;
                pointingAtController = null;
                HideInformationReadout();
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (pointingAtController != null)
                {
                    pointingAtController.Unhighlight();
                    pointingAtController.Interact();
                }
            }
        }
        else
        {
            Physics.Raycast(cameraRay, out rayHit, 100, dragLayer);
            pointingAtGameObject.transform.position = rayHit.point;

            if (Input.GetMouseButtonDown(0))
            {
                pointingAtController.Uninteract();
            }
        }

        if (pointingAtController != null)
        {
            UpdateInformationReadout();
        }
    }

    public void HoldObject(GameObject gameObject)
    {
        if (gameObject == null)
        {
            return;
        }

        isHoldingItem = true;
        pointingAtGameObject = gameObject;
        pointingAtController = gameObject.GetComponent<InteractionController>();

        Physics.Raycast(cameraRay, out rayHit, 100, dragLayer);
        pointingAtGameObject.transform.position = rayHit.point;
    }

    public void UnholdObject()
    {
        isHoldingItem = false;
        pointingAtGameObject = null;
        pointingAtController = null;
    }

    public void EnableInteraction()
    {
        enabled = true;
    }

    public void DisableInteraction()
    {
        enabled = false;
    }

    public void ShowInformationReadout()
    {
        switch (pointingAtController.controllerType)
        {
            case InteractionController.ControllerType.ADDITIVE_SOURCE:
                {
                    AdditiveSourceController additiveSourceController =
                        (AdditiveSourceController)pointingAtController;

                    Additive sourcedAdditive = additiveSourceController.containedAdditive;
                    AttributeModifier modifier = sourcedAdditive.initialEffect;

                    additiveReadoutController.gameObject.SetActive(true);

                    additiveReadoutController.SetAttributeFields(
                        modifier.Taste,
                        modifier.Strength,
                        modifier.Temperature);

                    break;
                }
            case InteractionController.ControllerType.CONTAINER:
                {
                    ContainerController containerController =
                        (ContainerController)pointingAtController;

                    switch (containerController.associatedInterface.interfaceType)
                    {
                        case Interface.InterfaceType.KETTLE_INTERFACE:
                            {
                                containerReadoutController.gameObject.SetActive(true);

                                KettleInterface kettleInterface =
                                    (KettleInterface)containerController.associatedInterface;

                                containerReadoutController.SetAttributeFields(
                                    0,
                                    0,
                                    kettleInterface.kettle.Temperature);
                                AttributeInfo info = kettleInterface.GetAttributeInfo();

                                if (info.containedAdditives == null)
                                {
                                    break;
                                }

                                containerReadoutController.SetAdditiveFields(info.containedAdditives.ToArray());

                                break;
                            }
                        case Interface.InterfaceType.TEAPOT_INTERFACE:
                            {
                                containerReadoutController.gameObject.SetActive(true);

                                TeapotInterface teapotInterface =
                                    (TeapotInterface)containerController.associatedInterface;

                                containerReadoutController.SetAttributeFields(
                                    teapotInterface.teapot.Taste,
                                    teapotInterface.teapot.Strength,
                                    teapotInterface.teapot.Temperature);
                                AttributeInfo info = teapotInterface.GetAttributeInfo();

                                if (info.containedAdditives == null)
                                {
                                    break;
                                }

                                containerReadoutController.SetAdditiveFields(info.containedAdditives.ToArray());

                                break;
                            }
                        case Interface.InterfaceType.CUP_INTERFACE:
                            {
                                containerReadoutController.gameObject.SetActive(true);

                                CupInterface cupInterface =
                                    (CupInterface)containerController.associatedInterface;

                                containerReadoutController.SetAttributeFields(
                                    cupInterface.cup.Taste,
                                    cupInterface.cup.Strength,
                                    cupInterface.cup.Temperature);
                                AttributeInfo info = cupInterface.GetAttributeInfo();

                                if (info.containedAdditives == null)
                                {
                                    break;
                                }

                                containerReadoutController.SetAdditiveFields(info.containedAdditives.ToArray());

                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }

                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    public void HideInformationReadout()
    {
        additiveReadoutController?.gameObject.SetActive(false);
        containerReadoutController?.gameObject.SetActive(false);
    }

    public void UpdateInformationReadout()
    {
        if (pointingAtController == null)
        {
            return;
        }

        switch (pointingAtController.controllerType)
        {
            case InteractionController.ControllerType.ADDITIVE_SOURCE:
                {
                    Vector3 screenCoordinates = Input.mousePosition;
                    additiveReadoutController.SetPosition(screenCoordinates);

                    break;
                }
            case InteractionController.ControllerType.CONTAINER:
                {
                    ContainerController containerController =
                        (ContainerController)pointingAtController;

                    switch (containerController.associatedInterface.interfaceType)
                    {
                        case Interface.InterfaceType.KETTLE_INTERFACE:
                            {
                                KettleInterface kettleInterface =
                                    (KettleInterface)containerController.associatedInterface;

                                containerReadoutController.SetAttributeFields(
                                    0,
                                    0,
                                    kettleInterface.kettle.Temperature);

                                Vector3 screenCoordinates = Input.mousePosition;
                                containerReadoutController.SetPosition(screenCoordinates);

                                break;
                            }
                        case Interface.InterfaceType.TEAPOT_INTERFACE:
                            {
                                TeapotInterface teapotInterface =
                                    (TeapotInterface)containerController.associatedInterface;

                                containerReadoutController.SetAttributeFields(
                                    teapotInterface.teapot.Taste,
                                    teapotInterface.teapot.Strength,
                                    teapotInterface.teapot.Temperature);

                                Vector3 screenCoordinates = Input.mousePosition;
                                containerReadoutController.SetPosition(screenCoordinates);

                                break;
                            }
                        case Interface.InterfaceType.CUP_INTERFACE:
                            {
                                CupInterface cupInterface =
                                    (CupInterface)containerController.associatedInterface;

                                containerReadoutController.SetAttributeFields(
                                    cupInterface.cup.Taste,
                                    cupInterface.cup.Strength,
                                    cupInterface.cup.Temperature);

                                Vector3 screenCoordinates = Input.mousePosition;
                                containerReadoutController.SetPosition(screenCoordinates);

                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }

                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}
