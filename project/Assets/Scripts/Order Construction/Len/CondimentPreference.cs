using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Timeline;
using UnityEngine;

[Serializable]
public class CondimentPreference
{
    #region Fields

    public string condimentName;

    public int additiveIndex;

    public float weighting;

    public float percentile;

    [HideInInspector]
    public bool customerSelected;

    #endregion

    #region Properties

    #endregion

    #region Functions

    public void Initialize()
    {
        additiveIndex = Additive.GetAdditive(condimentName).Index;
    }

    public static int CompareByIndex(CondimentPreference preference1, CondimentPreference preference2)
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

    public static int CompareByWeight(CondimentPreference preference1, CondimentPreference preference2)
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
