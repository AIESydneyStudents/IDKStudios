using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdditiveReadoutController : MonoBehaviour
{
    public RectTransform thisRect;
    public Text tasteField;
    public Text strengthField;
    public Text temperatureField;

    public Vector3 positionOffset;

    public void SetAttributeFields(float taste, float strength, float temperature)
    {
        tasteField.text = "Taste: " + taste.ToString();
        strengthField.text = "Strength: " + strength.ToString();
        temperatureField.text = "Temperature: " + temperature.ToString();
    }

    public void SetPosition(Vector3 screenPosition)
    {
        thisRect.anchoredPosition = screenPosition + positionOffset;
    }
}
