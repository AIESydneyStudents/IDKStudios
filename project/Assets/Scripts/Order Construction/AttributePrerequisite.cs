using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class AttributePrerequisite
{
    #region Fields

    [Tooltip("All taste values greater or equal are valid")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float tasteMin;

    [Tooltip("All taste values less or equal are valid")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float tasteMax;

    [Tooltip("All flavour values greater or equal are valid")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float flavourMin;

    [Tooltip("All flavour values less or equal are valid")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float flavourMax;

    [Tooltip("All temperature values greater or equal are valid")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float temperatureMin;

    [Tooltip("All temperature values less or equal are valid")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float temperatureMax;

    #endregion

    #region Properties

    #endregion

    #region Functions

    public bool IsTasteValid(float taste)
    {
        if (tasteMin <= tasteMax)
        {
            return (taste >= tasteMin && taste <= tasteMax);
        }
        else
        {
            return (taste <= tasteMax || taste >= tasteMin);
        }
    }

    public bool IsFlavourValid(float flavour)
    {
        if (flavourMin <= flavourMax)
        {
            return (flavour >= flavourMin && flavour <= flavourMax);
        }
        else
        {
            return (flavour <= flavourMax || flavour >= flavourMin);
        }
    }

    public bool IsTemperatureValid(float temperature)
    {
        if (temperatureMin <= temperatureMax)
        {
            return (temperature >= temperatureMin && temperature <= temperatureMax);
        }
        else
        {
            return (temperature <= temperatureMax || temperature >= temperatureMin);
        }
    }

    #endregion
}
