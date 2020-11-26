using UnityEngine;
using UnityEngine.UI;

public class TimeDisplayUI : MonoBehaviour
{
    public Text timeText;
    public Image faceIcon;

    public Sprite happyFace;
    public Sprite normalFace;
    public Sprite sadFace;

    public float counter;

    public int happyTime;
    public int normalTime;

    private void OnEnable()
    {
        faceIcon.sprite = happyFace;
        counter = 0;
        timeText.text = "0:00";
    }

    private void Update()
    {
        counter += Time.deltaTime;

        UpdateTime();
    }

    public void UpdateTime()
    {
        int time = (int)GameEventManager.Instance.missionTimer.ElapsedTime();
        int minutes = (int)(0.01667f * time);
        int seconds = time - minutes * 60;

        timeText.text = minutes.ToString() + " : " + (seconds < 10 ? "0" : "") + seconds.ToString();

        if (time < happyTime)
        {
            faceIcon.sprite = happyFace;
        }
        else if (time > happyTime + normalTime)
        {
            faceIcon.sprite = normalFace;
        }
        else
        {
            faceIcon.sprite = sadFace;
        }
    }
}
