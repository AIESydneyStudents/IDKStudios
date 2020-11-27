using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupInterface : Interface
{
    public Cup cup;

    private void Update()
    {
        cup.Simulate(Time.deltaTime);
    }

    public CupInterface()
    {
        interfaceType = InterfaceType.CUP_INTERFACE;
    }
}
