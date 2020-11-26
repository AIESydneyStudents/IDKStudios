using UnityEngine;
using UnityEngine.UI;

public class KettleUI : MonoBehaviour
{
    public AttributeSliderController temperatureSlider;
    public KettleInterface kettleInterface;
    public RectTransform thisRect;
    public Image cup1;

    public Sprite emptyCup;
    public Sprite fullCup;

    private void Update()
    {
        if (temperatureSlider == null)
        {
            return;
        }

        temperatureSlider.UpdateSlider(0, 0, kettleInterface.kettle.Temperature);
    }

    public void ShowUI(Vector3 screenPos)
    {
        thisRect.anchoredPosition = screenPos;

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }

    public void UpdateCupIcons()
    {
        cup1.sprite = kettleInterface.kettle.IsFull ? fullCup : emptyCup;
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }
}
