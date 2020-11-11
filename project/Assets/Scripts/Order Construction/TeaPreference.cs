using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[Serializable]
public class TeaPreference
{
    #region Fields

    public string teaName;

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
        additiveIndex = Additive.GetAdditive(teaName).Index;
    }

    public static int CompareByIndex(TeaPreference preference1, TeaPreference preference2)
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

    public static int CompareByWeight(TeaPreference preference1, TeaPreference preference2)
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
