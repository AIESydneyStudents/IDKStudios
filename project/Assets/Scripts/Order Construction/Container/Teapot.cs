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
    }

    public bool CanInsertAdditive(Additive additive)
    {
        if (containerType != additive.container)
        {
            return false;
        }

        if (teapotTaste + additive.initialEffect.Taste > 1)
        {
            return false;
        }

        if (teapotStrength + additive.initialEffect.Strength > 1)
        {
            return false;
        }

        if (teapotTemperature + additive.initialEffect.Temperature > 1)
        {
            return false;
        }

        // CHECK ATTRIBUTE PREREQUISITES

        if (!additive.attributePrerequisite.IsTasteValid(teapotTaste))
        {
            return false;
        }

        if (!additive.attributePrerequisite.IsStrengthValid(teapotStrength))
        {
            return false;
        }

        if (!additive.attributePrerequisite.IsTemperatureValid(teapotTemperature))
        {
            return false;
        }

        // CHECK ADDITIVE PREREQUISITES

        foreach (AdditivePrerequisite prerequisite in additive.additivePrerequisites)
        {

        }

        return true;
    }

    public void InsertAdditive(Additive additive)
    {
        if (!CanInsertAdditive(additive))
        {
            return;
        }

        InsertAdditiveToRepo(additive);

        // APPLY ATTRIBUTE EFFECTS
    }

    #endregion
}
