using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AdditiveStack
{
    #region Fields

    public int index;
    public int count;

    #endregion

    #region Properties

    public Additive Additive { get { return Additive.GetAdditive(index); } }

    #endregion

    #region Functions

    #endregion
}

