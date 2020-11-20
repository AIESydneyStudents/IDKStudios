using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributeUIController : MonoBehaviour
{
    public Slider strengthSlider;
    public Slider tasteSlider;
    public Slider temperatureSlider;

    public Text strengthValue;
    public Text tasteValue;
    public Text temperatureValue;

    // Update is called once per frame
    private void Update()
    {
        UpdateStrength();
        UpdateTaste();
        UpdateTemperature();
    }

    private void UpdateStrength()
    {
        float newValue = GameEventManager.Instance.cupController1.cup.Strength;
        strengthValue.text = newValue.ToString();
        strengthSlider.value = newValue;
    }

    private void UpdateTaste()
    {
        float newValue = GameEventManager.Instance.cupController1.cup.Taste; // needs to display text instead
        tasteValue.text = newValue.ToString();
        tasteSlider.value = newValue;
    }

    private void UpdateTemperature()
    {
        float newValue = GameEventManager.Instance.cupController1.cup.Temperature; // need to times by 100
        temperatureValue.text = newValue.ToString();
        temperatureSlider.value = GameEventManager.Instance.cupController1.cup.Temperature;
    }
}
