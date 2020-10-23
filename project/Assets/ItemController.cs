using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int count = Additive.AdditiveCount;
        Additive[] additives = Additive.GetAllAdditives();

        AdditiveStack stack0 = new AdditiveStack(0, 1);
        AdditiveStack stack1 = new AdditiveStack(1, 1);

        OrderProfile order = new OrderProfile();
        order.InsertAdditive(stack0);
        order.InsertAdditive(stack1, true);
    }
}
