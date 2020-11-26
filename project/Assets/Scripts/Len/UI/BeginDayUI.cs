using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginDayUI : MonoBehaviour
{
    public Text splashText;
    public float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > 2.0f)
        {
            timer = 0.0f;
            gameObject.SetActive(false);
            GameEventManager.Instance.SetEventToComplete();
        }
    }

    // Triggers a screen splash that fades in, shows the day beginning, and
    // fades back out. Once it has faded out, it needs to call the SetEventToComplete
    // function to trigger the next event.
    public void TriggerBeginDaySplash(int currentDay)
    {
        gameObject.SetActive(true);

        splashText.text = "DAY " + currentDay.ToString();
    }
}
