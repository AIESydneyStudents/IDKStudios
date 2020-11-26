using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditiveInterface : Interface
{
    // This is the additive that the gameObject that owns
    // this component should represent.
    public Additive containedAdditive;

    public AdditiveInterface()
    {
        interfaceType = InterfaceType.ADDITIVE_INTERFACE;
    }
}
