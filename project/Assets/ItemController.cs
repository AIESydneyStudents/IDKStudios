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
        Cup cup = cupObject.GetComponent<Cup>();
        Teapot teapot = teapotObject.GetComponent<Teapot>();
        Kettle kettle = kettleObject.GetComponent<Kettle>();

        if (true)
        {
            Additive[] additives = Additive.GetAllAdditives();

            kettle.FillFromTap(1);
            kettle.DispenseToTeapot(teapot);

            teapot.InsertAdditive(Additive.GetAdditive("Green Tea"));

            teapot.DispenseToCup(cup);
        }



    }
}
