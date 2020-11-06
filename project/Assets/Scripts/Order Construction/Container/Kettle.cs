using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class Kettle : Container
{
    #region Fields

    [Range(0, 2)]
    [SerializeField]
    [Tooltip("This is how much water is currently in the kettle.")]
    private int waterVolume = 0;
    private int maxWaterVolume = 2;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    [Tooltip("The temperature that the water currently in the kettle is at.")]
    private float kettleTemperature = 0;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    [Tooltip("How much temperature per second does the water contained cool.")]
    private float cooldownRate = 0;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    [Tooltip("How much temperature per second does the water contained heat.")]
    private float heatupRate = 0;

    [SerializeField]
    [Tooltip("Is the kettle active.")]
    private bool isActive = false;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    [Tooltip("Temperature setting. Will not heat if lower than kettle's current temperature.")]
    private float temperatureSetting = 0;

    #endregion

    #region Properties

    public float Temperature { get { return kettleTemperature; } }

    public float TemperatureSetting { get { return temperatureSetting; } set { temperatureSetting = value; } }

    public float WaterVolume { get { return waterVolume; } }

    public bool IsFull { get { return waterVolume == maxWaterVolume; } }

    public bool IsActive { get { return isActive; } set { isActive = value; } }

    #endregion

    #region Functions

    private void Update()
    {
        Cooldown();
        Heatup();
    }

    public void Cooldown()
    {
        if (waterVolume == 0)
        {
            return;
        }

        if (kettleTemperature <= 0)
        {
            return;
        }

        kettleTemperature -= cooldownRate * Time.deltaTime;
    }

    public void Heatup()
    {
        if (!isActive || waterVolume == 0)
        {
            return;
        }

        if (kettleTemperature >= temperatureSetting)
        {
            return;
        }

        kettleTemperature += heatupRate * Time.deltaTime;
    }

    public bool SetToActive()
    {
        if (waterVolume == 0)
        {
            return false;
        }

        isActive = true;

        return true;
    }

    public void SetToInactive()
    {
        isActive = false;
    }

    public bool CanDispenseToTeapot(Teapot teapot)
    {
        if (waterVolume == 0)
        {
            return false;
        }

        if (teapot.IsFull)
        {
            return false;
        }

        return true;
    }

    public void DispenseToTeapot(Teapot teapot)
    {
        if (!CanDispenseToTeapot(teapot))
        {
            return;
        }

        teapot.IsFull = true;

        teapot.Temperature = kettleTemperature;

        waterVolume--;

        if (waterVolume == 0)
        {
            kettleTemperature = 0;
            isActive = false;
        }
    }

    public bool CanFillFromTap(int volume)
    {
        if (IsFull)
        {
            return false;
        }

        if (waterVolume + volume > maxWaterVolume)
        {
            return false;
        }

        return true;
    }

    public void FillFromTap(int volume)
    {
        if (!CanFillFromTap(volume))
        {
            return;
        }

        switch (waterVolume)
        {
            case 0:
                kettleTemperature = 0;
                waterVolume = volume;
                break;

            default:
                int totalVolume = waterVolume + volume;
                kettleTemperature = kettleTemperature * waterVolume / totalVolume;
                waterVolume = totalVolume;
                break;
        }
    }

    #endregion
}
