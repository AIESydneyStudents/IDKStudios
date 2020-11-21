using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerUI : MonoBehaviour
{
    public Text customerName;
    public Text customerGreeting;
    public Image order1;
    public Image order2;

    public void ShowGreeting()
    {
        gameObject.SetActive(true);

        Customer customer = GameEventManager.Instance.openCustomer;
        int orderCount = GameEventManager.Instance.orderCount;

        customerName.text = customer.customerName;
        customerGreeting.text = customer.GetGreeting() + 
            orderCount.ToString() + " tea" + (orderCount == 2 ? "s" : "") + " please.";

        order2.gameObject.SetActive(orderCount == 2);
    }
}
