using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public struct AdditivePrerequisite : IComparable<AdditivePrerequisite>
{
    #region Fields

    [Tooltip("Name of additive")]
    public string additiveName;

    [Tooltip("All counts greater or equal are valid")]
    [Range(0.0f, 100.0f)]
    [SerializeField]
    private int countMin;

    [Tooltip("All counts less or equal are valid")]
    [Range(0.0f, 100.0f)]
    [SerializeField]
    private int countMax;

    private int additiveIndex;

    #endregion

    #region Properties

    public int Index { get { return additiveIndex; } }

    public int CountMin { get { return countMin; } }

    public int CountMax { get { return countMax; } }

    #endregion

    #region Functions

    // Needs to be run directly after this AdditivePrerequisite is
    // loaded into RAM.
    public void Initialize()
    {
        additiveIndex = Additive.GetAdditive(additiveName).Index;
    }

    // Checks if given count is within the specified range.
    public bool IsCountValid(int count)
    {
        if (countMin <= countMax)
        {
            return (count >= countMin && count <= countMax);
        }
        else
        {
            return (count <= countMax || count >= countMin);
        }
    }

    // Used for sorting additivePrerequisite array that belongs to 
    // each Additive.
    int IComparable<AdditivePrerequisite>.CompareTo(AdditivePrerequisite other)
    {
        return Index.CompareTo(other.Index);
    }

    #endregion
}
