using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupInterface : Interface
{
    public Cup cup;

    public MeshRenderer renderer;

    private void Update()
    {
        cup.Simulate(Time.deltaTime);

        Color color = renderer.material.color;
        color.r = cup.Temperature;
        renderer.material.color = color;
    }

    public CupInterface()
    {
        interfaceType = InterfaceType.CUP_INTERFACE;
    }
}
