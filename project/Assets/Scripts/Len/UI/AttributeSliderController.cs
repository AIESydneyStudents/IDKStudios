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

        offset = bar.rectTransform.anchoredPosition;
        offset.x = value * width;
        bar.rectTransform.anchoredPosition = offset;

        offset = tolerance.rectTransform.offsetMin;
        offset.x = width * (1.0f + toleranceMin);
        tolerance.rectTransform.offsetMin = offset;

        offset = tolerance.rectTransform.offsetMax;
        offset.x = width * (-1.0f + toleranceMax);
        tolerance.rectTransform.offsetMax = offset;
    }
}
