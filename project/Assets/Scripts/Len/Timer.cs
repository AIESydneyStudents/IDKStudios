using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    float seconds;
    bool enabled;

    private void Update()
    {
        if (!enabled)
        {
            return;
        }

        seconds += Time.deltaTime;
    }

    public void StartTimer()
    {
        seconds = 0;
        enabled = true;
    }

    public void PauseTimer()
    {
        enabled = false;
    }

    public void ResumeTimer()
    {
        enabled = true;
    }

    public float ElapsedTime()
    {
        return seconds;
    }
}
