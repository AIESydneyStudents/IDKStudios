using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Container
{
    #region Fields

    public enum Type
    {
        KETTLE,
        TEAPOT,
        CUP
    }

    [Tooltip("Which container is this?")]
    [SerializeField]
    public Type containerType;

    public List<Additive> additiveRepository =
        new List<Additive>();

    #endregion

    #region Properties

    #endregion

    #region Functions

    public bool ContainsAdditive(Additive additive)
    {
        return additiveRepository.Contains(additive);
    }

    public bool ContainsType(Additive.Type type)
    {
        foreach (Additive additive in additiveRepository)
        {
            if (additive.additiveType == type)
            {
                return true;
            }
        }

        return false;
    }

    public void InsertAdditiveToRepo(Additive additive)
    {
        additiveRepository.Add(additive);
    }

    protected void ResetContainer()
    {
        additiveRepository.Clear();
    }

    #endregion
}
