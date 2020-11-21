using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Slider))]
[RequireComponent(typeof(RectTransform))]

public class AttributeUIController : MonoBehaviour
{
    private Slider slider;
    private float sliderSize;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        UpdateSliderDirection();
    }

    // Updates the direction of the fill bar in the sliders
    public void UpdateSliderDirection()
    {
        if (sliderSize == 0)
        {
            sliderSize = GetComponent<RectTransform>().rect.width;
            sliderSize /= slider.maxValue - slider.minValue;
        }

        slider.fillRect.rotation = new Quaternion(180, 0, 0, 0);
        slider.fillRect.pivot = new Vector2(slider.fillRect.transform.parent.localPosition.x, slider.fillRect.pivot.y);

        if (slider.value > 0)
        {
            slider.fillRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sliderSize * slider.value);
        }

        else
        {
            slider.fillRect.Rotate(0, 0, 180);
            slider.fillRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, -1 * sliderSize * slider.value);
        }

        slider.fillRect.localPosition = new Vector3(0, 0, 0);
    }
}