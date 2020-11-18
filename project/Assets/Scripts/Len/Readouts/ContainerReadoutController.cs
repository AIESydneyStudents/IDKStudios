using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContainerReadoutController : MonoBehaviour
{
    public RectTransform thisRect;
    public Text additiveField;
    public GameObject additiveAnchor;
    public float listStart;

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

    public void SetAdditiveFields(string[] additiveStrings)
    {
        foreach (Transform child in additiveAnchor.transform)
        {
            Destroy(child.gameObject);
        }

        if (additiveStrings == null)
        {
            return;
        }

        for (int i = 0; i < additiveStrings.Length; i++)
        {
            Text newAdditiveField = Instantiate(additiveField, additiveAnchor.transform);
            newAdditiveField.text = "+ " + additiveStrings[i];
            Vector2 newFieldPosition;
            newFieldPosition.x = 0;
            newFieldPosition.y = listStart - i * additiveField.rectTransform.rect.height;
            newAdditiveField.rectTransform.anchoredPosition = newFieldPosition;
            newAdditiveField.gameObject.SetActive(true);
        }
    }

    public void SetPosition(Vector3 screenPosition)
    {
        thisRect.anchoredPosition = screenPosition + positionOffset;
    }
}
