using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Order
{
    #region Fields

    [Header("Target Order Attributes")]

    [Range(0.0f, 1.0f)]
    [SerializeField]
    public float targetTaste;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    public float targetStrength;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    public float targetTemperature;

    [Header("Attribute Tolerances")]

    [SerializeField]
    public float toleranceTaste;

    [SerializeField]
    public float toleranceStrength;

    [SerializeField]
    public float toleranceTemperature;

    public SortedSet<Additive> additiveRepository =
        new SortedSet<Additive>();

    public OrderEvaluation evaluation;

    #endregion

    #region Properties

    public bool IsEvaluated { get { return evaluation != null; } }

    #endregion

    #region Functions

    public Order(float toleranceTaste, float toleranceStrength, float toleranceTemperature)
    {
        this.toleranceTaste = toleranceTaste;
        this.toleranceStrength = toleranceStrength;
        this.toleranceTemperature = toleranceTemperature;
    }

    public void SetTarget(float taste, float strength, float temperature)
    {
        targetTaste = taste;
        targetStrength = strength;
        targetTemperature = temperature;
    }

    #endregion
}