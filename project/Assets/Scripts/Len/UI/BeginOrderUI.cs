using UnityEngine;
using UnityEngine.UI;

public class BeginOrderUI : MonoBehaviour
{
    public GameObject clockUI;
    public Text clockText;
    public DocketUI docketUI;

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
            clockUI.SetActive(true);
            clockText.text = "#";
            gameObject.SetActive(false);
            docketUI.ShowDockets();
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
    }
}
