using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class CollectiveWhitelistEntry
{
    #region Fields

    [Tooltip("The name of the colletive that this additive can " +
             "be a part of")]
    public string collectiveName;

    [Tooltip("The maximum amount of this additive that can be " +
             "added to the collective")]
    public int collectiveMax;

    private int collectiveIndex;

    #endregion

    #region Properties

    public int Index { get { return collectiveIndex; } }

    public int CollectiveMax { get { return collectiveMax; } }

    #endregion

    #region Functions

    // Sets up fields.
    public void Initialize()
    {
        collectiveIndex = Collective.GetCollective(collectiveName).Index;
    }

    #endregion
}