using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientUI : MonoBehaviour
{
    public Image ingredientIcon;
    public AttributeSliderController tasteSlider;
    public AttributeSliderController strengthSlider;
    public AttributeSliderController temperatureSlider;
    public RectTransform thisRect;

    public void SetAdditive(Additive additive)
    {
        ingredientIcon.sprite = additive.additiveSprite;
        tasteSlider.UpdateSlider(0, 0, additive.initialEffect.Taste);
        strengthSlider.UpdateSlider(0, 0, additive.initialEffect.Strength);
        temperatureSlider.UpdateSlider(0, 0, additive.initialEffect.Temperature);
    }

    public void ShowUI()
    {
        gameObject.SetActive(true);
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }

    public void UpdatePosition(Vector3 position)
    {
        thisRect.anchoredPosition = position;
    }
}
