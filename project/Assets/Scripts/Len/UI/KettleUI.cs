using UnityEngine;
using UnityEngine.UI;

public class KettleUI : MonoBehaviour
{
    public AttributeSliderController temperatureSlider;
    public KettleInterface kettleInterface;

    private void Update()
    {
        temperatureSlider.UpdateSlider(0, 0, kettleInterface.kettle.Temperature);
    }

    public void HideUI()
    {

    }
}
