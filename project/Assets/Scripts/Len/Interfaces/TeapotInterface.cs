using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeapotInterface : Interface
{
    public Teapot teapot;

    public GameObject validObject;
    private CupInterface cupInterface;

    private void Update()
    {
        teapot.Simulate(Time.deltaTime);
    }

    public TeapotInterface()
    {
        interfaceType = InterfaceType.TEAPOT_INTERFACE;
    }

    public bool SetValidObject(GameObject validObject)
    {
        if (validObject.TryGetComponent(out CupInterface cupInterface))
        {
            this.validObject = validObject;
            this.cupInterface = cupInterface;

            return true;
        }

        return false;
    }

    public void ClearValidObject()
    {
        validObject = null;
        cupInterface = null;
    }

    public bool CanDispenseToCup()
    {
        return teapot.CanDispenseToCup(cupInterface.cup);
    }

    public void DispenseToCup()
    {
        teapot.DispenseToCup(cupInterface.cup);
    }

    public AttributeInfo GetAttributeInfo()
    {
        AttributeInfo attributeInfo = new AttributeInfo();

        attributeInfo.infoTaste = teapot.Taste;
        attributeInfo.infoStrength = teapot.Strength;
        attributeInfo.infoTemperature = teapot.Temperature;

        attributeInfo.containedAdditives = new List<string>();

        foreach (Additive additive in teapot.additiveRepository)
        {
            attributeInfo.containedAdditives.Add(additive.name);
        }

        return attributeInfo;
    }
}
