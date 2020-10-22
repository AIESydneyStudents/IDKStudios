using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct AdditiveStack
{
    #region Fields

    private int index;
    private int count;
    private int countMax;

    #endregion

    #region Properties

    public int Index { get { return index; } }

    public int Count { get { return count; } }

    public int CountMax { get { return countMax; } }

    public Additive Additive { get { return Additive.GetAdditive(index); } }

    #endregion
}

