﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

[Serializable]
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

    public int WaterVolume { get { return waterVolume; } }

    public int WaterVolumeMax { get { return maxWaterVolume; } }

    public bool IsFull { get { return waterVolume == maxWaterVolume; } }

    public bool IsActive { get { return isActive; } set { isActive = value; } }

    #endregion

    #region Functions

    public Kettle()
    {
        containerType = Type.KETTLE;
    }

    public void Simulate(float deltaTime)
    {
        Cooldown(deltaTime);
        Heatup(deltaTime);
    }

    public void Cooldown(float deltaTime)
    {
        if (waterVolume == 0)
        {
            return;
        }

        if (kettleTemperature <= -1)
        {
            return;
        }

        if (isActive)
        {
            return;
        }

        kettleTemperature -= cooldownRate * deltaTime;
        kettleTemperature = Math.Max(kettleTemperature, -1.0f);
    }

    public void Heatup(float deltaTime)
    {
        if (!isActive || waterVolume == 0)
        {
            return;
        }

        if (kettleTemperature >= temperatureSetting)
        {
            isActive = false;
            return;
        }

        kettleTemperature += heatupRate * deltaTime;
        kettleTemperature = Math.Min(kettleTemperature, 1.0f);
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
            kettleTemperature = -1.0f;
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
                kettleTemperature = -1.0f;
                waterVolume = volume;
                break;

            default:
                int totalVolume = waterVolume + volume;
                kettleTemperature = (kettleTemperature * waterVolume / totalVolume) * 2 - 1;
                waterVolume = totalVolume;
                break;
        }
    }

    public void ResetKettle()
    {
        waterVolume = 0;
        kettleTemperature = -1.0f;
        isActive = false;
        temperatureSetting = -1.0f;
    }

    #endregion
}
