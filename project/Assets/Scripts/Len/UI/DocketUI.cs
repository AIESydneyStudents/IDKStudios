using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DocketUI : MonoBehaviour
{
    public Sprite anyTea;
    public Sprite anyCondiment;
    public Sprite anyMilk;

    public AttributeSliderController tasteSlider;
    public AttributeSliderController strengthSlider;
    public AttributeSliderController temperatureSlider;

    public Image teaAdditive;
    public Image condimentAdditive;
    public Image milkAdditive;

    public void ShowDocket()
    {
        Order order = GameEventManager.Instance.openOrder;

        Additive thisTeaAdditive = order.GetAdditiveOfType(Additive.Type.TEA);
        Additive thisCondimentAdditive = order.GetAdditiveOfType(Additive.Type.CONDIMENT);
        Additive thisMilkAdditive = order.GetAdditiveOfType(Additive.Type.MILK);

        teaAdditive.sprite = thisTeaAdditive == null ? anyTea : thisTeaAdditive.additiveSprite;
        condimentAdditive.sprite = thisCondimentAdditive == null ? anyCondiment : thisCondimentAdditive.additiveSprite;
        milkAdditive.sprite = thisMilkAdditive == null ? anyMilk : thisMilkAdditive.additiveSprite;

        tasteSlider.UpdateSlider(GameEventManager.Instance.MinTasteAdjusted, GameEventManager.Instance.MaxTasteAdjusted, 0.0f);
        strengthSlider.UpdateSlider(GameEventManager.Instance.MinStrengthAdjusted, GameEventManager.Instance.MaxStrengthAdjusted, 0.0f);
        temperatureSlider.UpdateSlider(GameEventManager.Instance.MinTemperatureAdjusted, GameEventManager.Instance.MaxTemperatureAdjusted, 0.0f);

        gameObject.SetActive(true);
    }
}