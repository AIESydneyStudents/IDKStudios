using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeapotInterface : MonoBehaviour
{
    private bool canUse = false;
    public Teapot teapot;

    public GameObject validObject;
    private CupInterface cupInterface;

    private MeshRenderer renderer;

    private void Start()
    {
        renderer = gameObject.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        teapot.Simulate(Time.deltaTime);

        Color color = renderer.material.color;
        color.r = teapot.Temperature;
        renderer.material.color = color;
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

        return attributeInfo;
    }

    private void OnMouseUp()
    {
        if (canUse)
        {
            DispenseToCup();

            canUse = false;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (SetValidObject(collider.gameObject))
        {
            if (CanDispenseToCup())
            {
                canUse = true;
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (!canUse)
        {
            ClearValidObject();
        }
    }
}
