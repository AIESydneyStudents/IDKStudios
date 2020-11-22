using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaucerMenuController : MenuController
{
    public CupInterface cupInterface;
    public AnchorController anchorController;

    public Order thisOrder;

    public SaucerMenuController()
    {
        menuType = MenuType.SAUCER_MENU;
    }

    public void SetOrder(Order order)
    {
        thisOrder = order;
    }
    
    public void ServeTea()
    {
        //GameEventManager.Instance.EvaluateOrder(thisOrder, cupInterface.cup);
        HideMenu();
    }

    public void ExitMenu()
    {
        HideMenu();
    }
}
