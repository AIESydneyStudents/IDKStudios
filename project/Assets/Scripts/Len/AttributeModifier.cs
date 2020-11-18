using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AttributeModifier
{
    #region Fields

    // The specific attributes.
    [Header("Additive Attributes")]

    [Range(-1.0f, 1.0f)]
    [SerializeField]
    private float effectTaste;

    [Range(-1.0f, 1.0f)]
    [SerializeField]
    private float effectStrength;

    [Range(-1.0f, 1.0f)]
    [SerializeField]
    private float effectTemperature;

    #endregion

    #region Properties

    public float Taste { get { return effectTaste; } }
    public float Strength { get { return effectStrength; } }
    public float Temperature { get { return effectTemperature; } }

    #endregion

    #region Functions

    public AttributeModifier(float taste, float strength, float temperature)
    {
        effectTaste = taste;
        effectStrength = strength;
        effectTemperature = temperature;
    }

    #endregion
}
