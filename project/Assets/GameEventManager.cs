using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : Singleton<GameEventManager>
{
    public List<Order> openCustomerOrders;
    public Customer openCustomer;

    public float EvaluateOrder(Order order)
    {
        if (openCustomer == null)
        {
            return 0.0f;
        }

        return 0.0f;
    }
}
