using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Cup : Container
{
    #region Fields

    [Tooltip("Shows if this cup is full of water or not.")]
    [SerializeField]
    private bool isFull;

    [SerializeField]
    private float cupTaste;

    [SerializeField]
    private float cupStrength;

    [SerializeField]
    private float cupTemperature;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    [Tooltip("How much temperature per second does the water contained cool.")]
    private float cooldownRate = 0;

    #endregion

    #region Properties

    public bool IsFull { get { return isFull; } set { isFull = value; } }

    public float Taste { get { return cupTaste; } set { cupTaste = value; } }

    public float Strength { get { return cupStrength; } set { cupStrength = value; } }

    public float Temperature { get { return cupTemperature; } set { cupTemperature = value; } }

    #endregion

    #region Functions

    public Cup()
    {
        containerType = Type.CUP;
    }

    private void Update()
    {
        Simulate(Time.deltaTime);
    }

    public void Simulate(float deltaTime)
    {
        Cooldown(deltaTime);
    }

    public void Cooldown(float deltaTime)
    {
        if (!isFull)
        {
            return;
        }

        if (cupTemperature <= 0)
        {
            return;
        }

        cupTemperature -= cooldownRate * deltaTime;
        cupTemperature = Math.Max(cupTemperature, 0.0f);
    }

    public float PreviewTaste(float taste)
    {
        return Math.Min(Math.Max(cupTaste + taste, -1.0f), 1.0f);
    }

    public float PreviewStrength(float strength)
    {
        return Math.Min(Math.Max(cupStrength + strength, 0.0f), 1.0f);
    }

    public float PreviewTemperature(float temperature)
    {
        return Math.Min(Math.Max(cupTemperature + temperature, 0.0f), 1.0f);
    }

    public bool CanInsertAdditive(Additive additive)
    {
        // Checks if additive can be added to this container.
        if (additive.container != containerType)
        {
            return false;
        }

        // If tea is required for additive, check if it contains tea.
        // Ignores check completely if it's tea in the first place.
        if (additive.additiveType == Additive.Type.CONDIMENT &&
            additive.teaRequirement &&
            !ContainsType(Additive.Type.TEA))
        {
            return false;
        }

        // Check if attribute ceiling will be hit by adding this additive.
        float resultTaste = cupTaste + additive.initialEffect.Taste;
        float resultStrength = cupStrength + additive.initialEffect.Strength;
        float resultTemperature = cupTemperature + additive.initialEffect.Temperature;

        if (resultTaste > 1.0f || resultTaste < -1.0f)
        {
            return false;
        }

        if (resultStrength > 1.0f || resultStrength < 0.0f)
        {
            return false;
        }

        if (resultTemperature > 1.0f || resultTemperature < 0.0f)
        {
            return false;
        }

        return true;
    }

    public void InsertAdditive(Additive additive)
    {
        // Check if additive can be inserted
        if (!CanInsertAdditive(additive))
        {
            return;
        }

        InsertAdditiveToRepo(additive);

        // Apply modifier
        cupTaste += additive.initialEffect.Taste;
        cupStrength += additive.initialEffect.Strength;
        cupTemperature += additive.initialEffect.Temperature;
    }

    #endregion
}

