using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KettleInterface : MonoBehaviour
{
    public Kettle kettle;

    private void Update()
    {
        kettle.Simulate(Time.deltaTime);
    }

    public AttributeInfo GetAttributeInfo()
    {
        AttributeInfo attributeInfo = new AttributeInfo();

        attributeInfo.infoTaste = 0;
        attributeInfo.infoStrength = 0;
        attributeInfo.infoTemperature = kettle.Temperature;

        return attributeInfo;
    }
}
