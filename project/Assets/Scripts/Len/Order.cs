using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Order
{
    #region Fields

    [Header("Target Order Attributes")]
    public float targetTaste;
    public float targetStrength;
    public float targetTemperature;

    public List<Additive> additiveRepository =
        new List<Additive>();

    public OrderEvaluation evaluation;

    #endregion

    #region Properties

    public bool IsEvaluated { get { return evaluation != null; } }

    #endregion

    #region Functions

    public Order(float taste, float strength, float temperature)
    {
        targetTaste = taste;
        targetStrength = strength;
        targetTemperature = temperature;
    }

    #endregion
}