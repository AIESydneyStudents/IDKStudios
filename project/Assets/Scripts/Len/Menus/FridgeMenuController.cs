using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeMenuController : MenuController
{
    public AdditiveSourceWithMenuController interactionController;

    public FridgeMenuController()
    {
        menuType = MenuType.FRIDGE_MENU;
    }

    public void SpawnObject1()
    {
        interactionController.SpawnObject1();
        HideMenu();
    }

    public void SpawnObject2()
    {
        interactionController.SpawnObject2();
        HideMenu();
    }

    public void ExitMenu()
    {
        HideMenu();
    }
}
