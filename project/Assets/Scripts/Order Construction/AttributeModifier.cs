using JetBrains.Annotations;
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
    private float effectFlavour;

    [Range(-1.0f, 1.0f)]
    [SerializeField]
    private float effectTemperature;

    #endregion

    #region Functions

    public AttributeModifier(float taste, float flavour, float temperature)
    {
        effectTaste = taste;
        effectFlavour = flavour;
        effectTemperature = temperature;
    }

    #endregion
}
