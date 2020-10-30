using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer
{
    [RuntimeInitializeOnLoadMethod(
        RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    private static void InitializeAll()
    {
        Collective.InitializeAll();
        Additive.InitializeAll();
    }
}
