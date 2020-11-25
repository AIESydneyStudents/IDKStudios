using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
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

    public void Simulate(float deltaTime)
    {
        Steep(deltaTime);
        Cooldown(deltaTime);
    }

    public void Steep(float deltaTime)
    {
        if (!isFull)
        {
            return;
        }

        foreach (Additive additive in additiveRepository)
        {
            teapotTaste       += additive.steepEffect.Taste       * deltaTime;
            teapotStrength    += additive.steepEffect.Strength    * deltaTime;
            teapotTemperature += additive.steepEffect.Temperature * deltaTime;

            teapotTaste       = Math.Max(-1.0f, Math.Min(1.0f, teapotTaste));
            teapotStrength    = Math.Max(-1.0f, Math.Min(1.0f, teapotStrength));
            teapotTemperature = Math.Max(-1.0f, Math.Min(1.0f, teapotTemperature));
        }
    }

    public void Cooldown(float deltaTime)
    {
        if (!isFull)
        {
            return;
        }

        if (teapotTemperature <= -1.0f)
        {
            return;
        }

        teapotTemperature -= cooldownRate * deltaTime;
        teapotTemperature = Math.Max(teapotTemperature, -1.0f);
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
        teapotStrength = -1.0f;
        teapotTemperature = -1.0f;

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
        if (additive.teaRequirement && 
            !ContainsType(Additive.Type.TEA))
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
            //return;
        }

        InsertAdditiveToRepo(additive);

        // Apply modifier
        teapotTaste += additive.initialEffect.Taste;
        teapotStrength += additive.initialEffect.Strength;
        teapotTemperature += additive.initialEffect.Temperature;

        teapotTaste = Mathf.Clamp(teapotTaste, -1.0f, 1.0f);
        teapotStrength = Mathf.Clamp(teapotStrength, -1.0f, 1.0f);
        teapotTemperature = Mathf.Clamp(teapotTemperature, -1.0f, 1.0f);
    }

    public void ResetTeapot()
    {
        isFull = false;
        teapotTaste = 0.0f;
        teapotStrength = 0.0f;
        teapotTemperature = 0.0f;
        ResetContainer();
    }

    #endregion
}
