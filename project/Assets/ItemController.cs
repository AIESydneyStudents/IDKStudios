using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Collective kettle = Collective.GetCollective("Kettle");
        Collective teapot = Collective.GetCollective("Teapot");

        AdditiveStack water = new AdditiveStack("Water", 5);
        AdditiveStack heat = new AdditiveStack("Heat", 8);

        kettle.InsertAdditive(ref water);
        kettle.InsertAdditive(ref heat);

        teapot.InsertAdditive(ref water);
        teapot.MergeCollective(kettle);
    }
}
