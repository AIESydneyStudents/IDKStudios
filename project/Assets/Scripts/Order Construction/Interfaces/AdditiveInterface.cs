using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class AdditiveInterface : MonoBehaviour
{
    private bool canInsert = false;

    // This is the additive that the gameObject that owns
    // this component should represent.
    public Additive containedAdditive;

    // These are the linked gameObject and interface that will
    // be set when additive object enters a valid containers space.
    // Eg. Moving milk carton into cup space.
    private GameObject containerObject;
    private TeapotInterface teapotInterface;
    private CupInterface cupInterface;

    // Will look and see if the gameObject is of the correct type.
    // Will return true if correct will return false if not. If it's true,
    // it will also grab the interface of that object.
    public bool SetContainerObject(GameObject containerObject)
    {
        switch (containedAdditive.container)
        {
            case Container.Type.TEAPOT:
                {
                    if (containerObject.TryGetComponent(out TeapotInterface teapotInterface))
                    {
                        this.containerObject = containerObject;
                        this.teapotInterface = teapotInterface;

                        return true;
                    }

                    return false;
                }
            case Container.Type.CUP:
                {
                    if (containerObject.TryGetComponent(out CupInterface cupInterface))
                    {
                        this.containerObject = containerObject;
                        this.cupInterface = cupInterface;

                        return true;
                    }

                    return false;
                }
            default:
                {
                    return false;
                }
        }
    }

    // Use this when additive gameObject is leaving the space of a container.
    public void ClearContainerObject()
    {
        containerObject = null;
        teapotInterface = null;
        cupInterface = null;
    }

    // Will check and see if this additive can be inserted into the container.
    // Will also set a reference attributeInfo as a preview of result if action
    // is confirmed.
    public bool CanInsertAdditive(ref AttributeInfo mergedInfo)
    {
        if (containerObject == null)
        {
            return false;
        }

        switch (containedAdditive.container)
        {
            case Container.Type.TEAPOT:
                {
                    if (teapotInterface.teapot.CanInsertAdditive(containedAdditive))
                    {
                        AttributeInfo teapotInfo = teapotInterface.GetAttributeInfo();

                        mergedInfo.infoTaste = containedAdditive.initialEffect.Taste + teapotInfo.infoTaste;
                        mergedInfo.infoStrength = containedAdditive.initialEffect.Strength + teapotInfo.infoStrength;
                        mergedInfo.infoTemperature = containedAdditive.initialEffect.Temperature + teapotInfo.infoTemperature;

                        return true;
                    }

                    return false;
                }
            case Container.Type.CUP:
                {
                    if (cupInterface.cup.CanInsertAdditive(containedAdditive))
                    {
                        AttributeInfo cupInfo = cupInterface.GetAttributeInfo();

                        mergedInfo.infoTaste = containedAdditive.initialEffect.Taste + cupInfo.infoTaste;
                        mergedInfo.infoStrength = containedAdditive.initialEffect.Strength + cupInfo.infoStrength;
                        mergedInfo.infoTemperature = containedAdditive.initialEffect.Temperature + cupInfo.infoTemperature;

                        return true;
                    }

                    return false;
                }
            default:
                {
                    return false;
                }
        }
    }

    // Gets the attribute info of the enclosed additive. Used to fill out the 
    // attribute tooltip that floats above an object being moved.
    public AttributeInfo GetAttributeInfo()
    {
        AttributeInfo attributeInfo = new AttributeInfo();

        attributeInfo.infoTaste = containedAdditive.initialEffect.Taste;
        attributeInfo.infoStrength = containedAdditive.initialEffect.Strength;
        attributeInfo.infoTemperature = containedAdditive.initialEffect.Temperature;

        return attributeInfo;
    }

    // Will initiate an insert.
    public void InsertAdditive()
    {
        if (containerObject == null)
        {
            return;
        }

        switch (containedAdditive.container)
        {
            case Container.Type.TEAPOT:
                {
                    teapotInterface.teapot.InsertAdditive(containedAdditive);
                    return;
                }
            case Container.Type.CUP:
                {
                    cupInterface.cup.InsertAdditive(containedAdditive);
                    return;
                }
            default:
                {
                    return;
                }
        }
    }

    private void OnMouseUp()
    {
        if (canInsert)
        {
            InsertAdditive();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Item")
        {
            if (SetContainerObject(collider.gameObject))
            {
                AttributeInfo info = new AttributeInfo();

                if (CanInsertAdditive(ref info))
                {
                    canInsert = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Item")
        {
            ClearContainerObject();
            canInsert = false;
        }
    }
}
