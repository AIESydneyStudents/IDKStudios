using System;
using UnityEngine;

[Serializable]
public class Kettle : Container
{
    #region Fields

    [SerializeField]
    private bool isFull;

    [Range(-1.0f, 1.0f)]
    [SerializeField]
    [Tooltip("The temperature that the water currently in the kettle is at.")]
    private float kettleTemperature = -1.0f;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    [Tooltip("How much temperature per second does the water contained cool.")]
    private float cooldownRate = 0;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    [Tooltip("How much temperature per second does the water contained heat.")]
    private float heatupRate = 0;

    [SerializeField]
    [Tooltip("Is the kettle active.")]
    private bool isActive = false;

    [Range(-1.0f, 1.0f)]
    [SerializeField]
    [Tooltip("Temperature setting. Will not heat if lower than kettle's current temperature.")]
    private float temperatureSetting = -1.0f;

    public AudioSource audioSource;

    public AudioClip heatupBegin;
    public AudioClip heatupDone;

    #endregion

    #region Properties

    public float Temperature { get { return kettleTemperature; } }

    public float TemperatureSetting { get { return temperatureSetting; } set { temperatureSetting = value; } }

    public bool IsFull { get { return isFull; } }

    public bool IsActive { get { return isActive; } set { isActive = value; } }

    #endregion

    #region Functions

    public Kettle()
    {
        containerType = Type.KETTLE;
    }

    public void Simulate(float deltaTime)
    {
        Cooldown(deltaTime);
        Heatup(deltaTime);
    }

    public void Cooldown(float deltaTime)
    {
        if (!isFull)
        {
            return;
        }

        if (kettleTemperature <= -1.0f)
        {
            return;
        }

        if (isActive)
        {
            return;
        }

        kettleTemperature -= cooldownRate * deltaTime;
        kettleTemperature = Math.Max(kettleTemperature, -1.0f);
    }

    public void Heatup(float deltaTime)
    {
        if (!isActive || !isFull)
        {
            return;
        }

        if (kettleTemperature >= temperatureSetting)
        {
            audioSource.Stop();
            audioSource.clip = heatupDone;
            audioSource.Play();
            //AudioSource.PlayClipAtPoint(heatupDone, Vector3.zero);
            isActive = false;
            return;
        }

        kettleTemperature += heatupRate * deltaTime;
        kettleTemperature = Math.Min(kettleTemperature, 1.0f);
    }

    public bool SetToActive()
    {
        if (!isFull)
        {
            return false;
        }

        audioSource.clip = heatupBegin;
        audioSource.Play();

        isActive = true;

        return true;
    }

    public void SetToInactive()
    {
        isActive = false;
    }

    public bool CanDispenseToTeapot(Teapot teapot)
    {
        if (!isFull)
        {
            return false;
        }

        if (teapot.IsFull)
        {
            return false;
        }

        return true;
    }

    public void DispenseToTeapot(Teapot teapot)
    {
        if (!CanDispenseToTeapot(teapot))
        {
            return;
        }

        teapot.IsFull = true;

        teapot.Temperature = kettleTemperature;

        isFull = false;
        kettleTemperature = -1.0f;
        isActive = false;
    }

    public bool CanFillFromTap()
    {
        if (isFull)
        {
            return false;
        }

        return true;
    }

    public void FillFromTap()
    {
        if (!CanFillFromTap())
        {
            return;
        }
        
        kettleTemperature = -1.0f;
        isFull = true;
    }

    public void ResetKettle()
    {
        isFull = false;
        isActive = false;
        kettleTemperature = -1.0f;
        temperatureSetting = -1.0f;
    }

    #endregion
}
