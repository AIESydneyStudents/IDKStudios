using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Container : MonoBehaviour
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

    protected SortedSet<Additive> additiveRepository =
        new SortedSet<Additive>();

    #endregion

    #region Properties

    #endregion

    #region Functions

    public bool ContainsAdditive(Additive additive)
    {
        return additiveRepository.Contains(additive);
    }

    public void InsertAdditiveToRepo(Additive additive)
    {
        additiveRepository.Add(additive);
    }

    public bool AdditivePrerequisitesMet(Additive additive)
    {
        return false;
    }

    #endregion
}
