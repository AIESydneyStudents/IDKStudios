using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    #region Fields

    [Header("Target Order Attributes")]

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float targetTaste;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float targetStrength;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float targetTemperature;

    [Header("Attribute Tolerances")]

    [SerializeField]
    private float toleranceTaste;

    [SerializeField]
    private float toleranceStrength;

    [SerializeField]
    private float toleranceTemperature;

    public SortedSet<Additive> additiveRepository =
        new SortedSet<Additive>();

    public bool isOpen;
    public float evaluationScore;
    public Customer belongsTo;

    #endregion

    #region Properties

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