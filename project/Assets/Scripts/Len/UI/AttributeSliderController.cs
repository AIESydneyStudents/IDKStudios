using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributeSliderController : MonoBehaviour
{
    public Image background;
    public Image tolerance;
    public Image bar;

    public void UpdateSlider(float toleranceMin, float toleranceMax, float value)
    {
        Vector2 offset;
        float width = background.rectTransform.rect.width * 0.5f;

        if (value < 0)
        {
            offset = bar.rectTransform.offsetMin;
            offset.x = width * (1.0f + value);
            bar.rectTransform.offsetMin = offset;
            offset = bar.rectTransform.offsetMax;
            offset.x = -width;
            bar.rectTransform.offsetMax = offset;
        }
        else if (value > 0)
        {
            offset = bar.rectTransform.offsetMax;
            offset.x = width * (-1.0f + value);
            bar.rectTransform.offsetMax = offset;
            offset = bar.rectTransform.offsetMin;
            offset.x = width;
            bar.rectTransform.offsetMin = offset;
        }

        offset = tolerance.rectTransform.offsetMin;
        offset.x = width * (1.0f + toleranceMin);
        tolerance.rectTransform.offsetMin = offset;


        offset = tolerance.rectTransform.offsetMax;
        offset.x = width * (-1.0f + toleranceMax);
        tolerance.rectTransform.offsetMax = offset;
    }
}
