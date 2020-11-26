using UnityEngine;
using UnityEngine.UI;

public class KettleUI : MonoBehaviour
{
    public Vector3 positionOffset;
    public AttributeSliderController temperatureSlider;
    public KettleInterface kettleInterface;
    public RectTransform thisRect;
    public Image cup;

    public Sprite emptyCup;
    public Sprite fullCup;

    private void OnEnable()
    {
        Vector3 kettleScreenPosition = InputController.Instance.camera.WorldToScreenPoint(kettleInterface.gameObject.transform.position);
        thisRect.anchoredPosition = kettleScreenPosition + positionOffset;
    }

    private void Update()
    {
        UpdateKettleUI();
    }

    public void UpdateKettleUI()
    {
        Vector3 kettleScreenPosition = InputController.Instance.camera.WorldToScreenPoint(kettleInterface.gameObject.transform.position);
        thisRect.anchoredPosition = kettleScreenPosition + positionOffset;

        temperatureSlider.UpdateSlider(0, 0, kettleInterface.kettle.Temperature);
    }

    public void UpdateIcons()
    {
        cup.sprite = kettleInterface.kettle.IsFull ? fullCup : emptyCup;
    }
}
