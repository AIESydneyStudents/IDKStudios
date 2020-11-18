using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Evaluation
{
    public enum Error
    {
        TOO_BITTER,
        TOO_SWEET,
        TOO_WEAK,
        TOO_STRONG,
        TOO_COLD,
        TOO_HOT,
        EMPTY_CUP,
        JUST_WATER,
        NO_ADDITIVE
    }

    public Error error;
    public string additiveName;

    public Evaluation(Error error, string additiveName = null)
    {
        this.error = error;
        this.additiveName = additiveName;
    }
}
