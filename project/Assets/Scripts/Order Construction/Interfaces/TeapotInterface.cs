using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeapotInterface : MonoBehaviour
{
    public Teapot teapot;

    private void Update()
    {
        teapot.Simulate(Time.deltaTime);
    }

    public AttributeInfo GetAttributeInfo()
    {
        AttributeInfo attributeInfo = new AttributeInfo();

        attributeInfo.infoTaste = teapot.Taste;
        attributeInfo.infoStrength = teapot.Strength;
        attributeInfo.infoTemperature = teapot.Temperature;

        return attributeInfo;
    }
}
