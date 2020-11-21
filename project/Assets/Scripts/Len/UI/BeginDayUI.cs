using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginDayUI : MonoBehaviour
{
    // GameObject references
    public Text splashText;

    public float fadeInTime;
    public float persistTime;
    public float fadeOutTime;

    private bool splashTriggered;
    public float splashTimer;


    private void Update()
    {
        if (!splashTriggered)
        {
            return;
        }

        // Run the fade in and fade out of the graphic.

        splashTimer += Time.deltaTime;

        if (splashTimer < fadeInTime)
        {
            Color color = splashText.color;
            color.a = splashTimer / fadeInTime;
            splashText.color = color;
        }
        else if (splashTimer > fadeInTime + persistTime)
        {
            Color color = splashText.color;
            color.a = 1.0f - (splashTimer - fadeInTime - persistTime) / fadeOutTime;
            splashText.color = color;
        }

        // End code
        if (splashTimer > fadeInTime + persistTime + fadeOutTime)
        {
            splashTriggered = false;
            gameObject.SetActive(false);
            GameEventManager.Instance.SetEventToComplete();
        }
    }

    // Triggers a screen splash that fades in, shows the day beginning, and
    // fades back out. Once it has faded out, it needs to call the SetEventToComplete
    // function to trigger the next event.
    public void TriggerBeginDaySplash(int day)
    {
        gameObject.SetActive(true);
        splashTriggered = true;
    }
}
