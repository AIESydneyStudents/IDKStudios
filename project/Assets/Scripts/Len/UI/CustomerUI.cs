using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerUI : MonoBehaviour
{
    public Text customerName;
    public Text customerGreeting;

    public void ShowGreeting()
    {
        gameObject.SetActive(true);

        Customer customer = GameEventManager.Instance.openCustomer;

        customerName.text = customer.customerName;
        customerGreeting.text = customer.GetGreeting();
    }
}
