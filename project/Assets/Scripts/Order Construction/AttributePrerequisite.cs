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

    [Tooltip("All strength values greater or equal are valid")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float strengthMin;

    [Tooltip("All strength values less or equal are valid")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float strengthMax;

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

    public bool IsStrengthValid(float strength)
    {
        if (strengthMin <= strengthMax)
        {
            return (strength >= strengthMin && strength <= strengthMax);
        }
        else
        {
            return (strength <= strengthMax || strength >= strengthMin);
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
