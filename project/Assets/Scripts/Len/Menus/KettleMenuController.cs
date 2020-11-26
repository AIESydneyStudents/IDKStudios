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
        tempOption1.interactable = kettleInterface.kettle.Temperature < -0.5f;
        tempOption2.interactable = kettleInterface.kettle.Temperature <  0.0f;
        tempOption3.interactable = kettleInterface.kettle.Temperature <  0.5f;
        tempOption4.interactable = kettleInterface.kettle.Temperature <  1.0f;
    }

    public void TempOption1()
    {
        kettleInterface.kettle.TemperatureSetting = -0.5f;
        kettleInterface.kettle.SetToActive();

        HideMenu();
        InputController.Instance.EnableInteraction();
        GameEventManager.Instance.kettleUI.gameObject.SetActive(true);
    }

    public void TempOption2()
    {
        kettleInterface.kettle.TemperatureSetting = 0.0f;
        kettleInterface.kettle.SetToActive();

        HideMenu();
        InputController.Instance.EnableInteraction();
        GameEventManager.Instance.kettleUI.gameObject.SetActive(true);
    }

    public void TempOption3()
    {
        kettleInterface.kettle.TemperatureSetting = 0.5f;
        kettleInterface.kettle.SetToActive();

        HideMenu();
        InputController.Instance.EnableInteraction();
        GameEventManager.Instance.kettleUI.gameObject.SetActive(true);
    }

    public void TempOption4()
    {
        kettleInterface.kettle.TemperatureSetting = 1.00f;
        kettleInterface.kettle.SetToActive();

        HideMenu();
        InputController.Instance.EnableInteraction();
        GameEventManager.Instance.kettleUI.gameObject.SetActive(true);
    }

    public void ExitMenu()
    {
        HideMenu();
        InputController.Instance.EnableInteraction();
        GameEventManager.Instance.kettleUI.gameObject.SetActive(true);
    }
}
