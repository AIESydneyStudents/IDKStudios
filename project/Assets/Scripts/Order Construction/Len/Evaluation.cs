﻿using System;
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
        NO_ADDITIVE
    }

    public Error error;
    public string additiveName;
}
