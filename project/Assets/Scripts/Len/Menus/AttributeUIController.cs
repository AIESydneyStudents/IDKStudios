using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]

public class AttributeUIController : MonoBehaviour
{
    private Slider _slider;
    private float center = 0.5f;

    void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    void Update()
    {
        _slider.fillRect.anchorMin = new Vector2(Mathf.Clamp(_slider.fillRect.anchorMin.x, 0, center), 0);
        _slider.fillRect.anchorMax = new Vector2(Mathf.Clamp(_slider.fillRect.anchorMin.x, center, 1), 1);
    }
}
