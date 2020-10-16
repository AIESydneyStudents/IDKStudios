using UnityEngine;

public class TimeController : Singleton<TimeController>
{
    [Tooltip("Sets game time scale. Each unit is a second. " +
             "Eg: 60 -> each real world second is a minute ingame.")]
    [SerializeField] private int timeScale = 60;
    [SerializeField] private bool timePaused = false;
    [SerializeField] private int dayStart = 0;
    [SerializeField] private int dayEnd = 10;
    private float timePassed;
    private float dayLength;
    private float dayLengthInv;
    private int currentSecond;
    private int currentMinute;
    private int currentHour;
    private int currentDay;

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

            if (timePassed > 1.0f)
            {
                currentSecond += (int)timePassed;
                timePassed -= (int)timePassed;

                if (currentSecond >= 60.0f)
                {
                    currentMinute += (int)(currentSecond * 0.01667f);
                    currentSecond = currentSecond % 60;

                    if (currentMinute >= 60.0f)
                    {
                        currentHour += (int)(currentMinute * 0.01667f);
                        currentMinute = currentMinute % 60;

                        if (currentHour >= dayLength)
                        {
                            currentDay += (int)(currentHour * dayLengthInv);
                            currentHour = currentHour % (int)dayLength;
                        }
                    }
                }
            }
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
        get { return TotalMinutes * 60 + currentSecond; }
    }

    public int Minute
    {
        get { return currentMinute; }
    }

    public int TotalMinutes
    {
        get { return TotalHours * 60 + currentMinute; }
    }

    public int Hour
    {
        get { return currentHour; }
    }

    public int TotalHours
    {
        get { return currentDay * (int)dayLength + currentHour; }
    }

    public int Day
    {
        get { return currentDay; }
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
            currentSecond = 0;
            currentMinute = 0;
            currentHour = 0;
            currentDay = 0;
        }
        else
        {
            currentSecond = 0;
            currentMinute = 0;
            currentHour = 0;
            currentDay = day;
        }
    }
}
