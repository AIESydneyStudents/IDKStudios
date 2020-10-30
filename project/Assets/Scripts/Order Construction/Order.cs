using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
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
}
