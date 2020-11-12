using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupInterface : MonoBehaviour
{
    public Cup cup;

    private void Update()
    {
        cup.Simulate(Time.deltaTime);
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
