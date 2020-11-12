using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupInterface : MonoBehaviour
{
    public Cup cup;

    public GameObject saucerObject;

    private SaucerInterface saucerInterface;

    private void Update()
    {
        cup.Simulate(Time.deltaTime);
    }

    public bool SetSaucerObject(GameObject saucerObject)
    {
        if (saucerObject.TryGetComponent(out SaucerInterface saucerInterface))
        {
            this.saucerObject = saucerObject;
            this.saucerInterface = saucerInterface;

            return true;
        }

        return false;
    }

    public void ClearSaucerObject()
    {
        saucerObject = null;
        saucerInterface = null;
    }

    public bool CanServeToSaucer()
    {
        return cup.IsFull;
    }

    public void ServeToSaucer()
    {
        // NEED CODE
    }

    public AttributeInfo GetAttributeInfo()
    {
        AttributeInfo attributeInfo = new AttributeInfo();

        attributeInfo.infoTaste = cup.Taste;
        attributeInfo.infoStrength = cup.Strength;
        attributeInfo.infoTemperature = cup.Temperature;

        return attributeInfo;
    }
}
