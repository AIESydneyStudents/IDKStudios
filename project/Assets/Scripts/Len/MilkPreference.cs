using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MilkPreference
{
    #region Fields

    public string milkName;

    public int additiveIndex;

    public float weighting;

    public float percentile;

    #endregion

    #region Properties

    #endregion

    #region Functions

    public void Initialize()
    {
        additiveIndex = Additive.GetAdditive(milkName).Index;
    }

    public static int CompareByIndex(MilkPreference preference1, MilkPreference preference2)
    {
        if (preference1.additiveIndex > preference2.additiveIndex)
        {
            return 1;
        }
        else if (preference1.additiveIndex == preference2.additiveIndex)
        {
            return 0;
        }
        else
        {
            return -1;
        }
    }

    public static int CompareByWeight(MilkPreference preference1, MilkPreference preference2)
    {
        if (preference1.weighting > preference2.weighting)
        {
            return 1;
        }
        else if (preference1.weighting == preference2.weighting)
        {
            return 0;
        }
        else
        {
            return -1;
        }
    }

    #endregion
}
