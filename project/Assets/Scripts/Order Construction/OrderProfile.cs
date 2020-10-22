using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderProfile
{
    #region Fields

    [Header("Current Order Attributes")]

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float currentTaste;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float currentFlavour;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float currentTemperature;

    [Header("Target Order Attributes")]

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float targetTaste;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float targetFlavour;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float targetTemperature;

    [Header("Attribute Tolerances")]

    [SerializeField]
    private float toleranceTaste;

    [SerializeField]
    private float toleranceFlavour;

    [SerializeField]
    private float toleranceTemperature;

    //NEED ingredientList

    #endregion
}
