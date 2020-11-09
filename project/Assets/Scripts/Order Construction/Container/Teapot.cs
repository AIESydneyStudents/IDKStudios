using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Teapot : Container
{
    #region Fields

    [Tooltip("Shows if this teapot is full of water or not. If full, additives " +
             "will steep.")]
    [SerializeField]
    private bool isFull;

    [SerializeField]
    private float teapotTaste;

    [SerializeField]
    private float teapotStrength;

    [SerializeField]
    private float teapotTemperature;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    [Tooltip("How much temperature per second does the water contained cool.")]
    private float cooldownRate = 0;

    #endregion

    #region Properties

    public bool IsFull { get { return isFull; } set { isFull = value; } }

    public float Taste { get { return teapotTaste; } }

    public float Strength { get { return teapotStrength; } }

    public float Temperature { get { return teapotTemperature; } set { teapotTemperature = value; } }

    #endregion

    #region Functions

    public Teapot()
    {
        containerType = Type.TEAPOT;
    }

    private void Update()
    {
        Steep();
        Cooldown();
    }

    private void Steep()
    {
        foreach (Additive additive in additiveRepository)
        {
            float delta = Time.deltaTime;

            teapotTaste       += additive.steepEffect.Taste       * delta;
            teapotStrength    += additive.steepEffect.Strength    * delta;
            teapotTemperature += additive.steepEffect.Temperature * delta;

            teapotTaste       = Math.Max(-1.0f, Math.Min(1.0f, teapotTaste));
            teapotStrength    = Math.Max( 0.0f, Math.Min(1.0f, teapotStrength));
            teapotTemperature = Math.Max( 0.0f, Math.Min(1.0f, teapotTemperature));
        }
    }

    private void Cooldown()
    {
        if (!isFull)
        {
            return;
        }

        if (teapotTemperature <= 0)
        {
            return;
        }

        teapotTemperature -= cooldownRate * Time.deltaTime;
        teapotTemperature = Math.Max(teapotTemperature, 0.0f);
    }

    public bool CanDispenseToCup(Cup cup)
    {
        if (!isFull)
        {
            return false;
        }

        if (cup.IsFull)
        {
            return false;
        }

        return true;
    }

    public void DispenseToCup(Cup cup)
    {
        if (!CanDispenseToCup(cup))
        {
            return;
        }

        cup.IsFull = true;

        cup.Taste = teapotTaste;
        cup.Strength = teapotStrength;
        cup.Temperature = teapotTemperature;

        isFull = false;

        teapotTaste = 0;
        teapotStrength = 0;
        teapotTemperature = 0;

        foreach (Additive additive in additiveRepository)
        {
            cup.InsertAdditiveToRepo(additive);
        }

        additiveRepository.Clear();
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
        float resultTaste = teapotTaste + additive.initialEffect.Taste;
        float resultStrength = teapotStrength + additive.initialEffect.Strength;
        float resultTemperature = teapotTemperature + additive.initialEffect.Temperature;

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
        teapotTaste += additive.initialEffect.Taste;
        teapotStrength += additive.initialEffect.Strength;
        teapotTemperature += additive.initialEffect.Temperature;
    }

    #endregion
}
