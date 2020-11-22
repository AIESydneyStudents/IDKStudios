using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeDisplayUI : MonoBehaviour
{
    public Text timeText;
    public Image pauseImage;

    public bool isPaused;
    public float counter;
    public bool pauseShown;

    public int greenTime;
    public int yellowTime;

    private void OnEnable()
    {
        isPaused = false;
        timeText.text = "0:00";
    }

    private void Update()
    {
        if (isPaused)
        {
            counter += Time.deltaTime;

            if (pauseShown)
            {
                if (counter > 0.5f)
                {
                    pauseShown = false;
                    counter = 0.0f;
                    pauseImage.gameObject.SetActive(false);
                }
            }
            else
            {
                if (counter > 0.5f)
                {
                    pauseShown = true;
                    counter = 0.0f;
                    pauseImage.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            UpdateTime();
        }
    }

    public void UpdateTime()
    {
        int time = (int)GameEventManager.Instance.missionTimer.ElapsedTime();
        int minutes = (int)(0.01667f * time);
        int seconds = time - minutes * 60;

        timeText.text = minutes.ToString() + ":" + seconds.ToString();

        if (time < greenTime)
        {
            timeText.color = Color.green;
        }
        else if (time > greenTime + yellowTime)
        {
            timeText.color = Color.red;
        }
        else
        {
            timeText.color = Color.yellow;
        }
    }

    public void ShowPause(bool value)
    {
        isPaused = value;
        
        if (!value)
        {
            pauseImage.gameObject.SetActive(false);
        }
        else
        {
            counter = 0.0f;
            pauseShown = true;
        }
    }
}
