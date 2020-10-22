using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public struct AdditivePrerequisite
{
    [Tooltip("Name of additive")]
    public string additiveName;

    [Tooltip("All counts greater or equal to this count are valid")]
    [Range(0.0f, 100.0f)]
    public int countMin;

    [Tooltip("All counts less or equal to this count are valid")]
    [Range(0.0f, 100.0f)]
    public int countMax;

    private int additiveIndex;

    public void Initialize()
    {
        additiveIndex = Additive.GetAdditive(additiveName).Index;
    }
}
