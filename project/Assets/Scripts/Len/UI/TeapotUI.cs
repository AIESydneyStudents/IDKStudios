using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeapotUI : MonoBehaviour
{
    public AttributeSliderController tasteSlider;
    public AttributeSliderController strengthSlider;
    public AttributeSliderController temperatureSlider;

    public Image ingredientIcon;
    public Image cupIcon;

    public Sprite emptyCup;
    public Sprite fullCup;

    public TeapotInterface teapotInterface;
    public RectTransform thisRect;

    private void Update()
    {
        if (teapotInterface == null)
        {
            return;
        }

        tasteSlider.UpdateSlider(0, 0, teapotInterface.teapot.Taste);
        strengthSlider.UpdateSlider(0, 0, teapotInterface.teapot.Strength);
        temperatureSlider.UpdateSlider(0, 0, teapotInterface.teapot.Temperature);
    }

    public void ShowUI(Vector3 screenPos)
    {
        thisRect.anchoredPosition = screenPos;

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }

    public void SetInterface(TeapotInterface teapotInterface)
    {
        this.teapotInterface = teapotInterface;
    }

    public void UpdateCupIcons()
    {
        cupIcon.sprite = teapotInterface.teapot.IsFull ? fullCup : emptyCup;
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }
}
