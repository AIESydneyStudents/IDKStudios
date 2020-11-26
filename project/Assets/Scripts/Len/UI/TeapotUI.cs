using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeapotUI : MonoBehaviour
{
    public AttributeSliderController tasteSlider;
    public AttributeSliderController strengthSlider;
    public AttributeSliderController temperatureSlider;

    public Image teaIcon;
    public Image cupIcon;

    public Sprite noTea;
    public Sprite emptyCup;
    public Sprite fullCup;

    public TeapotInterface teapotInterface;

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
        tasteSlider.UpdateSlider(0, 0, teapotInterface.teapot.Taste);
        strengthSlider.UpdateSlider(0, 0, teapotInterface.teapot.Strength);
        temperatureSlider.UpdateSlider(0, 0, teapotInterface.teapot.Temperature);
    }

    public void UpdateIcons()
    {
        cupIcon.sprite = teapotInterface.teapot.IsFull ? fullCup : emptyCup;
        teaIcon.sprite = teapotInterface.teapot.additiveRepository.Count == 0 ? noTea :
            teapotInterface.teapot.additiveRepository[0].additiveSprite;
    }
}
