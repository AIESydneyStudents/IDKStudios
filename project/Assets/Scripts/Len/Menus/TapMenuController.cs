using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapMenuController : MenuController
{
    public KettleInterface kettleInterface;
    public InteractionController kettleController;
    public Button cupOne;
    public Button cupTwo;

    private void OnEnable()
    {
        ShowFillOptions();
    }

    public TapMenuController()
    {
        menuType = MenuType.TAP_MENU;
    }

    public void ShowFillOptions()
    {
        cupOne.interactable = true;
        cupTwo.interactable = kettleInterface.kettle.WaterVolume == 0;
    }

    public void FillOne()
    {
        kettleInterface.kettle.FillFromTap(1);

        HideMenu();

        kettleController.ReturnToAnchor();
    }

    public void FillTwo()
    {
        kettleInterface.kettle.FillFromTap(2);

        HideMenu();

        kettleController.ReturnToAnchor();
    }

    public void ExitMenu()
    {
        HideMenu();

        kettleController.ReturnToAnchor();
    }
}
