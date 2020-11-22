﻿using UnityEngine;
using UnityEngine.UI;

public class DocketUI : MonoBehaviour
{
    public GameObject docket1;
    public GameObject docket2;
    public Button docket1Button;
    public Button docket2Button;

    #region Docket 1 Data
    [Header("Docket 1")]
    public Slider strengthSlider1;
    public Slider tasteSlider1;
    public Slider temperatureSlider1;

    #endregion

    #region Docket 2 Data
    [Header("Docket 2")]
    public Slider strengthSlider2;
    public Slider tasteSlider2;
    public Slider temperatureSlider2;

    #endregion

    // Update is called once per frame
    private void Update()
    {
        //UpdateDockets();
    }

    public void ShowDockets()
    {
        gameObject.SetActive(true);

        docket1.SetActive(true);

        if (GameEventManager.Instance.order2 != null)
        {
            docket2.SetActive(true);
        }
    }

    public void UpdateDockets()
    {
        // Strength Slider
        strengthSlider1.value = GameEventManager.Instance.cupInterface1.cup.Strength;

        // Taste Slider
        tasteSlider1.value = GameEventManager.Instance.cupInterface1.cup.Taste;

        // Temperature Slider
        temperatureSlider1.value = GameEventManager.Instance.cupInterface1.cup.Temperature;

        if (GameEventManager.Instance.order2 != null)
        {
            // Strength Slider
            strengthSlider2.value = GameEventManager.Instance.cupInterface2.cup.Strength;

            // Taste Slider
            tasteSlider2.value = GameEventManager.Instance.cupInterface2.cup.Taste;

            // Temperature Slider
            temperatureSlider2.value = GameEventManager.Instance.cupInterface2.cup.Temperature;
        }
    }

    public void HideDocketSubmit()
    {
        docket1Button.gameObject.SetActive(false);
        docket2Button.gameObject.SetActive(false);
    }

    public void ShowDocketSubmit()
    {
        docket1Button.gameObject.SetActive(true);
        docket2Button.gameObject.SetActive(true);
    }
}