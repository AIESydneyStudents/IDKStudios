using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public GameObject cupObject;
    public GameObject teapotObject;
    public GameObject kettleObject;


    // Start is called before the first frame update
    void Start()
    {
        Customer[] customers = Customer.GetAllCustomers();

        Order order = customers[0].GenerateOrder();
    }
}
