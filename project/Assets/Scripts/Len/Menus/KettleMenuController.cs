using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KettleMenuController : MenuController
{
    public KettleInterface kettleInterface;
    public ContainerController kettleController;
    public InteractionController kettleBaseController;
    public Button tempOption1;
    public Button tempOption2;
    public Button tempOption3;
    public Button tempOption4;

    private void OnEnable()
    {
        ShowTempOptions();
    }

    public KettleMenuController()
    {
        menuType = MenuType.KETTLE_MENU;
    }

    public void ShowTempOptions()
    {
        tempOption1.interactable = kettleInterface.kettle.Temperature < 0.25f && kettleInterface.kettle.WaterVolume > 0;
        tempOption2.interactable = kettleInterface.kettle.Temperature < 0.50f && kettleInterface.kettle.WaterVolume > 0;
        tempOption3.interactable = kettleInterface.kettle.Temperature < 0.75f && kettleInterface.kettle.WaterVolume > 0;
        tempOption4.interactable = kettleInterface.kettle.Temperature < 1.00f && kettleInterface.kettle.WaterVolume > 0;
    }

    public void TempOption1()
    {
        kettleInterface.kettle.TemperatureSetting = 0.25f;
        kettleInterface.kettle.SetToActive();

        HideMenu();
    }

    public void TempOption2()
    {
        kettleInterface.kettle.TemperatureSetting = 0.50f;
        kettleInterface.kettle.SetToActive();

        HideMenu();
    }

    public void TempOption3()
    {
        kettleInterface.kettle.TemperatureSetting = 0.75f;
        kettleInterface.kettle.SetToActive();

        HideMenu();
    }

    public void TempOption4()
    {
        kettleInterface.kettle.TemperatureSetting = 1.00f;
        kettleInterface.kettle.SetToActive();

        HideMenu();
    }

    public void ExitMenu()
    {
        HideMenu();
    }
}
