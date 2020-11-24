using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public enum ControllerType
    {
        ADDITIVE,
        ADDITIVE_SOURCE,
        ADDITIVE_SOURCE_WITH_MENU,
        CONTAINER,
        ANCHOR
    }

    public static Dictionary<GameObject, InteractionController> allControllers =
        new Dictionary<GameObject, InteractionController>();

    public Dictionary<GameObject, InteractionController> validControllers =
        new Dictionary<GameObject, InteractionController>();

    public ControllerType controllerType;

    public MeshRenderer meshRenderer;
    public Collider interactionTrigger;
    public bool isHighlighted;

    public Color originalColor;
    public Color hoverHighlightColor;
    public Color interactHightlightColor;

    public GameObject compatibleGameObject;
    public InteractionController compatibleController;

    public MenuController menuController;
    public ProgressBarController progressBarController;
    public Vector3 progressBarOffset;

    public AnchorController anchorController;
    public bool atAnchor;

    public void Start()
    {
        allControllers.Add(gameObject, this);
        originalColor = meshRenderer.material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (compatibleGameObject != null)
        {
            return;
        }

        if (validControllers.ContainsKey(other.gameObject))
        {
            compatibleGameObject = other.gameObject;
            compatibleController = validControllers[other.gameObject];
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (compatibleGameObject == null)
        {
            return;
        }

        if (compatibleGameObject == other.gameObject)
        {
            compatibleGameObject = null;
            compatibleController = null;
        }
    }

    public virtual void EnableCollisionTrigger()
    {
        interactionTrigger.enabled = true;
    }

    public virtual void DisableCollisionTrigger()
    {
        interactionTrigger.enabled = false;
    }

    public virtual void HoverHighlight()
    {
        isHighlighted = true;
        meshRenderer.material.color = hoverHighlightColor;
    }

    public virtual void InteractHightlight()
    {
        isHighlighted = true;
        meshRenderer.material.color = interactHightlightColor;
    }

    public virtual void Unhighlight()
    {
        isHighlighted = false;
        meshRenderer.material.color = originalColor;
    }

    public void FindValidControllers()
    {
        validControllers.Clear();

        switch (controllerType)
        {
            case ControllerType.ADDITIVE:
                {
                    AdditiveController additiveController =
                                        (AdditiveController)this;

                    AdditiveInterface additiveInterface =
                        additiveController.additiveInterface;

                    Additive additive = additiveInterface.containedAdditive;

                    foreach (InteractionController controller in allControllers.Values)
                    {
                        if (controller.controllerType != ControllerType.CONTAINER)
                        {
                            continue;
                        }

                        ContainerController containerController = (ContainerController)controller;

                        switch (containerController.associatedInterface.interfaceType)
                        {
                            case Interface.InterfaceType.TEAPOT_INTERFACE:
                                {
                                    TeapotInterface teapotInterface =
                                        (TeapotInterface)containerController.associatedInterface;

                                    if (teapotInterface.teapot.CanInsertAdditive(additive))
                                    {
                                        validControllers.Add(containerController.gameObject, containerController);
                                        continue;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            case Interface.InterfaceType.CUP_INTERFACE:
                                {
                                    CupInterface cupInterface =
                                        (CupInterface)containerController.associatedInterface;

                                    if (cupInterface.cup.CanInsertAdditive(additive))
                                    {
                                        validControllers.Add(containerController.gameObject, containerController);
                                        continue;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            default:
                                {
                                    continue;
                                }
                        }
                    }

                    return;
                }
            case ControllerType.CONTAINER:
                {
                    ContainerController containerController =
                        (ContainerController)this;

                    switch (containerController.associatedInterface.interfaceType)
                    {
                        case Interface.InterfaceType.KETTLE_INTERFACE:
                            {
                                KettleInterface kettleInterface =
                                    (KettleInterface)containerController.associatedInterface;

                                foreach (InteractionController controller in allControllers.Values)
                                {
                                    if (controller.controllerType != ControllerType.CONTAINER)
                                    {
                                        continue;
                                    }

                                    ContainerController otherContainerController =
                                        (ContainerController)controller;

                                    switch (otherContainerController.associatedInterface.interfaceType)
                                    {
                                        case Interface.InterfaceType.TAP_INTERFACE:
                                            {
                                                TapInterface tapInterface =
                                                    (TapInterface)otherContainerController.associatedInterface;

                                                if (kettleInterface.kettle.CanFillFromTap(1))
                                                {
                                                    validControllers.Add(tapInterface.gameObject, otherContainerController);
                                                    continue;
                                                }
                                                else
                                                {
                                                    continue;
                                                }
                                            }
                                        case Interface.InterfaceType.TEAPOT_INTERFACE:
                                            {
                                                TeapotInterface teapotInterface =
                                                    (TeapotInterface)otherContainerController.associatedInterface;

                                                if (kettleInterface.kettle.CanDispenseToTeapot(teapotInterface.teapot))
                                                {
                                                    validControllers.Add(teapotInterface.gameObject, otherContainerController);
                                                    continue;
                                                }
                                                else
                                                {
                                                    continue;
                                                }
                                            }
                                        default:
                                            {
                                                continue;
                                            }
                                    }
                                }

                                return;
                            }
                        case Interface.InterfaceType.TEAPOT_INTERFACE:
                            {
                                TeapotInterface teapotInterface =
                                    (TeapotInterface)containerController.associatedInterface;

                                foreach (InteractionController controller in allControllers.Values)
                                {
                                    if (controller.controllerType != ControllerType.CONTAINER)
                                    {
                                        continue;
                                    }

                                    ContainerController otherContainerController =
                                        (ContainerController)controller;

                                    switch (otherContainerController.associatedInterface.interfaceType)
                                    {

                                        case Interface.InterfaceType.TAP_INTERFACE:
                                            {
                                                TapInterface tapInterface =
                                                    (TapInterface)otherContainerController.associatedInterface;

                                                if (teapotInterface.teapot.IsFull)
                                                {
                                                    validControllers.Add(tapInterface.gameObject, otherContainerController);
                                                    continue;
                                                }
                                                else
                                                {
                                                    continue;
                                                }

                                            }
                                        case Interface.InterfaceType.CUP_INTERFACE:
                                            {
                                                if (otherContainerController.associatedInterface.interfaceType !=
                                                    Interface.InterfaceType.CUP_INTERFACE)
                                                {
                                                    continue;
                                                }

                                                CupInterface cupInterface =
                                                    (CupInterface)otherContainerController.associatedInterface;

                                                if (teapotInterface.teapot.CanDispenseToCup(cupInterface.cup))
                                                {
                                                    validControllers.Add(cupInterface.gameObject, otherContainerController);
                                                    continue;
                                                }
                                                else
                                                {
                                                    continue;
                                                }
                                            }
                                        default:
                                            {
                                                break;
                                            }
                                    }
                                }

                                return;
                            }
                        case Interface.InterfaceType.CUP_INTERFACE:
                            {
                                CupInterface cupInterface =
                                    (CupInterface)containerController.associatedInterface;

                                foreach (InteractionController controller in allControllers.Values)
                                {
                                    if (controller.controllerType != ControllerType.CONTAINER)
                                    {
                                        continue;
                                    }

                                    ContainerController otherContainerController =
                                        (ContainerController)controller;
                                    switch (otherContainerController.associatedInterface.interfaceType)
                                    {

                                        case Interface.InterfaceType.TAP_INTERFACE:
                                            {
                                                TapInterface tapInterface =
                                                    (TapInterface)otherContainerController.associatedInterface;

                                                if (cupInterface.cup.IsFull)
                                                {
                                                    validControllers.Add(tapInterface.gameObject, otherContainerController);
                                                    continue;
                                                }
                                                else
                                                {
                                                    continue;
                                                }

                                            }
                                        default:
                                            {
                                                break;
                                            }
                                    }
                                }

                                return;
                            }
                        default:
                            {
                                return;
                            }
                    }
                }
            default:
                {
                    return;
                }
        }
    }

    public void ClearValidControllers()
    {
        validControllers.Clear();
    }

    public void HighlightValidControllers()
    {
        if (validControllers.Count == 0)
        {
            return;
        }

        foreach (InteractionController controller in validControllers.Values)
        {
            controller.HoverHighlight();
        }
    }

    public void UnhighlightValidControllers()
    {
        if (validControllers.Count == 0)
        {
            return;
        }

        foreach (InteractionController controller in validControllers.Values)
        {
            controller.Unhighlight();
        }
    }

    public virtual void Interact()
    {

    }

    public virtual void Uninteract()
    {

    }

    public void ReturnToAnchor()
    {
        if (anchorController == null)
        {
            return;
        }

        Transform anchorTransform = anchorController.GetAnchorTransform();
        Transform thisTransform = gameObject.transform;

        thisTransform.position = anchorTransform.position;
        thisTransform.rotation = anchorTransform.rotation;

        anchorController.visitingController = this;
        atAnchor = true;

        compatibleGameObject = null;
        compatibleController = null;
    }

    public void LeaveAnchor()
    {
        if (anchorController == null)
        {
            return;
        }

        anchorController.visitingController = null;
        atAnchor = false;
    }

    public virtual void ShowProgressBar()
    {

    }

    public virtual void HideProgressBar()
    {
        progressBarController.HideProgressBar();
    }

    public virtual void UpdateProgressBar()
    {

    }

    //#if UNITY_EDITOR
    //    private void OnGUI()
    //    {
    //        Vector2 cam = Camera.main.WorldToScreenPoint(transform.position);
    //        cam.y = Screen.height - cam.y;
    //        Rect rect = new Rect(cam, new Vector2(200, 200));
    //        GUI.Label(rect, gameObject.name);
    //    }
    //#endif
}
