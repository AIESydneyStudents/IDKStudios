using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TimeController : Singleton<TimeController>
{
    [Tooltip("Sets game time scale. Each unit is a second. " +
             "Eg: 60 -> each real world second is a minute ingame.")]
    [SerializeField] private int m_TimeScale;
    private float m_TimePassed;

    [SerializeField] private bool m_TimePaused = false;

    private int m_DayStart;
    private int m_DayEnd;
    private float m_DayLengthInv;

    private int m_CurrentSecond;
    private int m_CurrentMinute;
    private int m_CurrentHour;
    private int m_CurrentDay;

    private void Awake()
    {
        m_DayLengthInv = 1.0f / (m_DayEnd - m_DayStart);
    }

    private void Update()
    {
        if (!m_TimePaused)
        {
            m_TimePassed += Time.deltaTime * m_TimeScale;
        }
    }

    public int TimeScale
    {
        get
        {
            return m_TimeScale;
        }
        set
        {
            m_TimeScale = value;
        }
    }

    public int Seconds
    {
        get
        {
            return m_CurrentSecond;
        }
    }

    public int TotalSeconds
    {
        get
        {
            return 0;
        }
    }

    public int Minutes
    {
        get
        {
            return m_CurrentMinute;
        }
    }

    public int TotalMinutes
    {
        get
        {
            return 0;
        }
    }

    public int Hours
    {
        get
        {
            return m_CurrentHour;
        }
    }

    public int TotalHours
    {
        get
        {
            return 0;
        }
    }

    public int Day
    {
        get
        {
            return m_CurrentDay;
        }
    }

    public int TotalDays
    {
        get
        {
            return 0;
        }
    }

    public float DayProgress
    {
        get
        {
            return (Hours - m_DayStart) * m_DayLengthInv;
        }
    }

    public void PauseTime()
    {
        m_TimePaused = true;
    }

    public void ResumeTime()
    {
        m_TimePaused = false;
    }

    public void ResetTime()
    {
        m_TimePassed = 0;
    }

    public void StartNextDay()
    {

    }
}
