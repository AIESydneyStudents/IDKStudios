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

    public KettleUI kettleUI;
    public TeapotUI teapotUI;

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
                        HideInformationReadout();
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
                    HideInformationReadout();
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
            HideInformationReadout();

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
            case InteractionController.ControllerType.CONTAINER:
                {
                    ContainerController containerController =
                        (ContainerController)pointingAtController;

                    switch (containerController.associatedInterface.interfaceType)
                    {
                        case Interface.InterfaceType.KETTLE_INTERFACE:
                            {

                                break;
                            }
                        case Interface.InterfaceType.TEAPOT_INTERFACE:
                            {
                                TeapotInterface teapotInterface =
                                    (TeapotInterface)containerController.associatedInterface;

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
    }

    public void UpdateInformationReadout()
    {
        
    }
}
