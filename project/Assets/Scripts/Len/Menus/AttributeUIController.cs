using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributeUIController : MonoBehaviour
{
    public CupInterface cupController;

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
        float newValue = cupController.cup.Strength;
        strengthValue.text = newValue.ToString();
        strengthSlider.value = newValue;

        //if (newValue == 0 || )
        //{
        //
        //}
    }

    private void UpdateTaste()
    {
        float newValue = cupController.cup.Taste; // needs to display text instead
        tasteValue.text = newValue.ToString();
        tasteSlider.value = newValue;
    }

    private void UpdateTemperature()
    {
        float newValue = cupController.cup.Temperature * 100; // need to times by 100
        int temp = (int)newValue;
        temperatureValue.text = temp.ToString();
        temperatureSlider.value = newValue;
    }
}
