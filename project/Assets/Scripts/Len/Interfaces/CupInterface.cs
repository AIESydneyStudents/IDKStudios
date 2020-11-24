using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupInterface : Interface
{
    public Cup cup;

    public int cupIndex;

    public GameObject saucerObject;

    private SaucerInterface saucerInterface;

    private MeshRenderer renderer;

    private void Start()
    {
        renderer = gameObject.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        cup.Simulate(Time.deltaTime);

        Color color = renderer.material.color;
        color.r = cup.Temperature;
        renderer.material.color = color;
    }

    public CupInterface()
    {
        interfaceType = InterfaceType.CUP_INTERFACE;
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

        attributeInfo.containedAdditives = new List<string>();

        foreach (Additive additive in cup.additiveRepository)
        {
            attributeInfo.containedAdditives.Add(additive.name);
        }

        return attributeInfo;
    }
}
