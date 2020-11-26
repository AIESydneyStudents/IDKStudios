using UnityEngine;
using UnityEngine.UI;

public class BeginOrderUI : MonoBehaviour
{
    public bool triggered;
    public float triggerTimer;

    private void Update()
    {
        if (!triggered)
        {
            return;
        }

        triggerTimer += Time.deltaTime;

        if (triggerTimer > 2.0f)
        {
            triggerTimer = 0.0f;
            gameObject.SetActive(false);

            // Setup for gameplay
            GameEventManager.Instance.missionTimer.StartTimer();
            GameEventManager.Instance.timeDisplay.gameObject.SetActive(true);

            GameEventManager.Instance.SetEventToComplete();
        }
    }

    public void TriggerBeginOrderSwipe()
    {
        // Tilt camera down to look at table.
        // Show clock
        // Show "MAKE TEA!"
        // Enable interaction

        gameObject.SetActive(true);
        triggered = true;
        GameEventManager.Instance.TriggerCameraZoomIn();
    }
}
