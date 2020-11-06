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

    private void Update()
    {
        Cooldown();
    }

    private void Cooldown()
    {
        if (!isFull)
        {
            return;
        }

        if (cupTemperature <= 0)
        {
            return;
        }

        cupTemperature -= cooldownRate * Time.deltaTime;
    }

    public bool CanInsertAdditive(Additive additive)
    {
        // Check if can insert

        return false;
    }

    public void InsertAdditive(Additive additive)
    {
        if (!CanInsertAdditive(additive))
        {
            return;
        }

        // Insert code
    }

    public void ServeToOrder(Order order)
    {

    }

    #endregion
}

