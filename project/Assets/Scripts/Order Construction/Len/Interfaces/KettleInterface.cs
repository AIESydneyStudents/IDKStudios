﻿using UnityEngine;

public class KettleInterface : MonoBehaviour
{
    private bool canUse = false;
    public Kettle kettle;

    public GameObject validObject;
    private TapInterface tapInterface;
    private TeapotInterface teapotInterface;

    private MeshRenderer renderer;

    private void Start()
    {
        renderer = gameObject.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        kettle.Simulate(Time.deltaTime);

        Color color = renderer.material.color;
        color.r = kettle.Temperature;
        renderer.material.color = color;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            kettle.IsActive = true;
        }
    }

    public bool SetValidObject(GameObject validObject)
    {
        if (kettle.WaterVolume == 0)
        {
            if (validObject.TryGetComponent(out TapInterface tapInterface))
            {
                this.validObject = validObject;
                this.tapInterface = tapInterface;

                return true;
            }
        }
        else if (kettle.WaterVolume == kettle.WaterVolumeMax)
        {
            if (validObject.TryGetComponent(out TeapotInterface teapotInterface))
            {
                this.validObject = validObject;
                this.teapotInterface = teapotInterface;

                return true;
            }
        }
        else
        {
            if (validObject.TryGetComponent(out TapInterface tapInterface))
            {
                this.validObject = validObject;
                this.tapInterface = tapInterface;

                return true;
            }

            if (validObject.TryGetComponent(out TeapotInterface teapotInterface))
            {
                this.validObject = validObject;
                this.teapotInterface = teapotInterface;

                return true;
            }
        }

        return false;
    }

    public void ClearValidObject()
    {
        validObject = null;
        tapInterface = null;
        teapotInterface = null;
    }

    public bool CanFillFromTap(int volume, ref int newVolume)
    {
        if (tapInterface == null)
        {
            return false;
        }

        if (!kettle.CanFillFromTap(volume))
        {
            return false;
        }

        newVolume = kettle.WaterVolume + volume;

        return true;
    }

    public void FillFromTap(int volume)
    {
        kettle.FillFromTap(volume);
    }

    public bool CanDispenseToTeapot()
    {
        return kettle.CanDispenseToTeapot(teapotInterface.teapot);
    }

    public void DispenseToTeapot()
    {
        kettle.DispenseToTeapot(teapotInterface.teapot);
    }

    public AttributeInfo GetAttributeInfo()
    {
        AttributeInfo attributeInfo = new AttributeInfo();

        attributeInfo.infoTaste = 0;
        attributeInfo.infoStrength = 0;
        attributeInfo.infoTemperature = kettle.Temperature;

        return attributeInfo;
    }

    private void OnMouseUp()
    {
        if (canUse)
        {
            if (teapotInterface != null)
            {
                DispenseToTeapot();
            }

            else
            {
                // FIX VALUE
                FillFromTap(1);
            }

            canUse = false;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (SetValidObject(collider.gameObject))
        {
            if (teapotInterface != null)
            {
                if (CanDispenseToTeapot())
                {
                    canUse = true;
                }
            }

            else
            {
                int rubbish = 0;

                if (CanFillFromTap(1, ref rubbish))
                {
                    canUse = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        ClearValidObject();
        canUse = false;
    }
}
