using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    public RectTransform thisRect;
    public Image background;
    public Image bar;
    public float borderWidth;

    public float start;
    public float end;
    public float current;

    public Color startColor;
    public Color endColor;

    public bool initialized;
    public bool finished;

    private void Start()
    {
        thisRect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (end <= current)
        {
            initialized = false;
            HideProgressBar();
        }

        if (initialized)
        {
            float progress = (current - start) / (end - start);
            Color progressColor = (endColor - startColor) * progress + startColor;
            progressColor.a = 1.0f;
            Vector2 offset = bar.rectTransform.offsetMax;
            float width = background.rectTransform.rect.width;
            offset.x = progress * (width - 2 * borderWidth) - width;
            bar.rectTransform.offsetMax = offset;
            bar.color = progressColor;
        }
    }

    public void SetStartEnd(float start, float end)
    {
        this.start = start;
        this.end = end;

        initialized = true;
    }

    public void SetProgress(float currentProgress)
    {
        current = currentProgress;
    }

    public void SetPosition(Vector3 screenPosition)
    {
        thisRect.anchoredPosition = screenPosition;
    }

    public void ShowProgressBar()
    {
        if (!initialized)
        {
            return;
        }

        gameObject.SetActive(true);
    }

    public void HideProgressBar()
    {
        initialized = false;
        gameObject.SetActive(false);
    }
}
