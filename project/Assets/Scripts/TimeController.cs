using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Sprites;
using UnityEngine;

public class TimeController : Singleton<TimeController>
{
    [Tooltip("Sets game time scale. Each unit is a second. " +
             "Eg: 60 -> each real world second is a minute ingame.")]
    [SerializeField] private int timeScale;
    private float timePassedPrev;
    private float timePassed;

    [SerializeField] private bool timePaused = false;
    [SerializeField] private int dayStart;
    [SerializeField] private int dayEnd;

    private float dayLength;
    private float dayLengthInv;
    private float dayRemainder;

    private int prevSecond;
    private int prevMinute;
    private int prevHour;
    private int prevDay;

    [SerializeField] private int currentSecond;
    [SerializeField] private int currentMinute;
    [SerializeField] private int currentHour;
    [SerializeField] private int currentDay;

    private void Awake()
    {
        dayLength = dayEnd - dayStart;
        dayLengthInv = 1.0f / dayLength;
    }

    private void Update()
    {
        if (!timePaused)
        {
            timePassed += Time.deltaTime * timeScale;
            dayRemainder = timePassed;

            currentDay = (int)(dayRemainder * 0.000278f * dayLengthInv);
            dayRemainder = currentDay * 3600 * dayLength;

            currentHour = (int)(dayRemainder * 0);
        }
    }

    public int TimeScale
    {
        get { return timeScale; }
        set { timeScale = value; }
    }

    public int Second
    {
        get { return currentSecond; }
    }

    public int TotalSeconds
    {
        get { return (int)(timePassed * timeScale); }
    }

    public int Minute
    {
        get { return currentMinute; }
    }

    public int TotalMinutes
    {
        get { return (int)(TotalSeconds * 0.1666f); }
    }

    public int Hour
    {
        get { return currentHour; }
    }

    public int TotalHours
    {
        get { return (int)(TotalMinutes * 0.1666f); }
    }

    public int Day
    {
        get { return currentDay; }
    }

    public int TotalDays
    {
        get { return (int)(TotalHours * dayLengthInv); }
    }

    /// <summary>
    /// Gives a value between 0 and 1 that reflects the 
    /// progress through the game day window.
    /// </summary>
    public float DayProgress
    {
        get { return (Hour - dayStart) * dayLengthInv; }
    }

    /// <summary>
    /// Pauses game time.
    /// </summary>
    public void PauseTime()
    {
        timePaused = true;
    }

    /// <summary>
    /// Resumes game time.
    /// </summary>
    public void ResumeTime()
    {
        timePaused = false;
    }

    /// <summary>
    /// Resets time back to startHour of Day 1
    /// </summary>
    public void ResetTime()
    {
        timePassed = 0;
    }

    /// <summary>
    /// If 'a_Day' is not set or 0, next available day will start. 
    /// Otherwise specified day will start.
    /// </summary>
    public void StartDay(int day = 0)
    {
        if (day == 0)
        {
            timePassed = 0;
        }
        else 
        { 
            timePassed = (Day + 1) * (dayEnd - dayStart) * 3600; 
        }
    }
}
