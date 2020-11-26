using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeapotInterface : Interface
{
    public Teapot teapot;

    private void Update()
    {
        teapot.Simulate(Time.deltaTime);
    }

    public TeapotInterface()
    {
        interfaceType = InterfaceType.TEAPOT_INTERFACE;
    }
}
