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
    public Text temperature;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateStrength();
        UpdateTaste();
        UpdateTemperature();
    }

    private void UpdateStrength()
    {
        strengthSlider.value = GameEventManager.Instance.cupController1.cup.Strength;
    }

    private void UpdateTaste()
    {
        tasteSlider.value = GameEventManager.Instance.cupController1.cup.Taste;
    }

    private void UpdateTemperature()
    {
        temperatureSlider.value = GameEventManager.Instance.cupController1.cup.Temperature;
    }
}
