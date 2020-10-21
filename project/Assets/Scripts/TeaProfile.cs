using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeaProfile
{
    #region Fields

    private float taste;
    private float flavour;
    private float temperature;

    #endregion

    #region Functions

    public TeaProfile(float taste, float flavour, float temperature)
    {
        this.taste = taste;
        this.flavour = flavour;
        this.temperature = temperature;
    }

    public static TeaProfile operator +(TeaProfile lhs, TeaProfile rhs)
    {
        return new TeaProfile(
            Math.Min(Math.Max(lhs.taste       + rhs.taste,       0), 1),
            Math.Min(Math.Max(lhs.flavour     + rhs.flavour,     0), 1),
            Math.Min(Math.Max(lhs.temperature + rhs.temperature, 0), 1));
    }

    #endregion
}
