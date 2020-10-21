using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    [Header("Target Order Attributes")]

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float tasteTarget;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float flavourTarget;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float temperatureTarget;

    [Header("Current Order Attributes")]

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float tasteCurrent;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float flavourCurrent;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float temperatureCurrent;
}
