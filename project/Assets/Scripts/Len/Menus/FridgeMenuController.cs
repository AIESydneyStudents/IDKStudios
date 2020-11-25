using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FridgeMenuController : MenuController
{
    public AdditiveSourceWithMenuController interactionController;
    public IngredientUI ingredientUI;

    public Button additive1Button;
    public Button additive2Button;

    public FridgeMenuController()
    {
        menuType = MenuType.FRIDGE_MENU;
    }

    public void SpawnObject1()
    {
        interactionController.SpawnObject1();
        HideMenu();
        ingredientUI.HideUI();
    }

    public void SpawnObject2()
    {
        interactionController.SpawnObject2();
        HideMenu();
        ingredientUI.HideUI();
    }

    public void ExitMenu()
    {
        HideMenu();
        ingredientUI.HideUI();
    }
}
