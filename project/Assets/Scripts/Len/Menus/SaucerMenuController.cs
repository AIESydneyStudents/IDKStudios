using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaucerMenuController : MenuController
{
    public CupInterface cupInterface;
    public AnchorController anchorController;

    private Order thisOrder;

    
    public void ServeTea()
    {
        GameEventManager.Instance.EvaluateOrder(thisOrder, cupInterface.cup);
    }

    public void ExitMenu()
    {
        HideMenu();
    }
}
