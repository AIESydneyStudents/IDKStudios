using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DocketUI : MonoBehaviour
{
    public GameObject docket1;
    public GameObject docket2;

    #region Docket 1 Sliders
    [Header("Docket 1 Sliders")]
    public Slider strengthSlider1;
    public Slider tasteSlider1;
    public Slider temperatureSlider1;

    #endregion

    #region Docket 2 Sliders
    [Header("Docket 2 Sliders")]
    public Slider strengthSlider2;
    public Slider tasteSlider2;
    public Slider temperatureSlider2;

    #endregion

    // Update is called once per frame
    private void Update()
    {
        //UpdateSliderDirection();
        //ShowDockets();
    }

    public void ShowDockets()
    {
        gameObject.SetActive(true);

        docket1.SetActive(true);

        if (false)
        {
            // Strength Slider
            strengthSlider1.value = GameEventManager.Instance.cupInterface1.cup.Strength;

            // Taste Slider
            tasteSlider1.value = GameEventManager.Instance.cupInterface1.cup.Taste;

            // Temperature Slider
            temperatureSlider1.value = GameEventManager.Instance.cupInterface1.cup.Temperature;
        }

        if (GameEventManager.Instance.order2 != null)
        {
            docket2.SetActive(true);

            if (false)
            {
                // Strength Slider
                strengthSlider2.value = GameEventManager.Instance.cupInterface2.cup.Strength;

                // Taste Slider
                tasteSlider2.value = GameEventManager.Instance.cupInterface2.cup.Taste;

                // Temperature Slider
                temperatureSlider2.value = GameEventManager.Instance.cupInterface2.cup.Temperature;
            }
        }
    }

    private void UpdateSliderDirection()
    {
        if (strengthSlider1.value > 0)
        {
            strengthSlider1.fillRect.anchorMin = new Vector2(0, 0);
            strengthSlider1.fillRect.anchorMax = new Vector2(strengthSlider1.fillRect.anchorMin.x, 1);
        }
        else
        {
            strengthSlider1.fillRect.anchorMin = new Vector2(strengthSlider1.fillRect.anchorMin.x, 0);
            strengthSlider1.fillRect.anchorMax = new Vector2(-1f, 1);
        }

        if (tasteSlider1.value > 0)
        {
            tasteSlider1.fillRect.anchorMin = new Vector2(0, 0);
            tasteSlider1.fillRect.anchorMax = new Vector2(tasteSlider1.fillRect.anchorMin.x, 1);
        }
        else
        {
            tasteSlider1.fillRect.anchorMin = new Vector2(tasteSlider1.fillRect.anchorMin.x, 0);
            tasteSlider1.fillRect.anchorMax = new Vector2(-1f, 1);
        }

        if (temperatureSlider1.value > 0)
        {
            temperatureSlider1.fillRect.anchorMin = new Vector2(0, 0);
            temperatureSlider1.fillRect.anchorMax = new Vector2(temperatureSlider1.fillRect.anchorMin.x, 1);
        }
        else
        {
            temperatureSlider1.fillRect.anchorMin = new Vector2(temperatureSlider1.fillRect.anchorMin.x, 0);
            temperatureSlider1.fillRect.anchorMax = new Vector2(-1f, 1);
        }
    }
}