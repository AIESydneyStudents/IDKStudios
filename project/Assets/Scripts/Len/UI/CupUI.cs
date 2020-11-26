using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CupUI : MonoBehaviour
{
    public AttributeSliderController tasteSlider;
    public AttributeSliderController strengthSlider;
    public AttributeSliderController temperatureSlider;

    public Image teaIcon;
    public Image condimentIcon;
    public Image milkIcon;
    public Image cupIcon;

    public Sprite noIngredient;
    public Sprite emptyCup;
    public Sprite fullCup;

    public CupInterface cupInterface;

    private void OnEnable()
    {
        UpdateSliders();
        UpdateIcons();
    }

    private void Update()
    {
        UpdateSliders();
    }

    public void UpdateSliders()
    {
        tasteSlider.UpdateSlider(0, 0, cupInterface.cup.Taste);
        strengthSlider.UpdateSlider(0, 0, cupInterface.cup.Strength);
        temperatureSlider.UpdateSlider(0, 0, cupInterface.cup.Temperature);
    }

    public void UpdateIcons()
    {
        cupIcon.sprite = cupInterface.cup.IsFull ? fullCup : emptyCup;

        Additive teaAdditive = cupInterface.cup.GetType(Additive.Type.TEA);
        Additive condimentAdditive = cupInterface.cup.GetType(Additive.Type.CONDIMENT);
        Additive milkAdditive = cupInterface.cup.GetType(Additive.Type.MILK);

        teaIcon.sprite = teaAdditive == null ? noIngredient : teaAdditive.additiveSprite;
        condimentIcon.sprite = condimentAdditive == null ? noIngredient : condimentAdditive.additiveSprite;
        milkIcon.sprite = milkAdditive == null ? noIngredient : milkAdditive.additiveSprite;
    }
}
