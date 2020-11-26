using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DocketUI : MonoBehaviour
{
    public GameObject docket1;

    public Button docket1Button;

    public Sprite noAdditive;
    public Sprite cupEmpty;
    public Sprite cupFull;
    public Sprite tick;
    public Sprite cross;

    #region Docket 1 Data
    [Header("Docket 1")]

    public AttributeSliderController tasteSlider1;
    public AttributeSliderController strengthSlider1;
    public AttributeSliderController temperatureSlider1;

    public List<Additive> additivesList1 = new List<Additive>();
    public Image[] additives1;
    public Image[] ticks1;
    public Image capacity1;

    #endregion

    // Update is called once per frame
    private void Update()
    {
        if (GameEventManager.Instance.currentEvent != GameEventManager.GameEvent.MAKE_ORDER)
        {
            return;
        }

        UpdateDockets();
    }

    public void ShowDockets()
    {

        gameObject.SetActive(true);

        docket1.SetActive(true);
    }

    public void UpdateDockets()
    {
        #region Docket1 Sliders

        Cup cup1 = GameEventManager.Instance.cupInterface1.cup;

        tasteSlider1.UpdateSlider(
            GameEventManager.Instance.MinTasteAdjusted1,
            GameEventManager.Instance.MaxTasteAdjusted1,
            cup1.Taste);

        strengthSlider1.UpdateSlider(
            GameEventManager.Instance.MinStrengthAdjusted1,
            GameEventManager.Instance.MaxStrengthAdjusted1,
            cup1.Strength);

        temperatureSlider1.UpdateSlider(
            GameEventManager.Instance.MinTemperatureAdjusted1,
            GameEventManager.Instance.MaxTemperatureAdjusted1,
            cup1.Temperature);

        #endregion
    }

    public void HideDocketSubmit()
    {
        docket1Button.gameObject.SetActive(false);
    }

    public void ShowDocketSubmit()
    {
        docket1Button.gameObject.SetActive(true);
    }

    public void InsertAdditive(Additive additive, int docket)
    {

        switch (additive.additiveType)
        {
            case Additive.Type.TEA:
                {
                    if (additivesList1[0].Index == additive.Index)
                    {
                        ticks1[0].sprite = tick;

                    }
                    else
                    {
                        ticks1[0].sprite = cross;
                    }

                    Color color = ticks1[0].color;
                    color.a = 0.5f;
                    ticks1[0].color = color;

                    break;
                }
            case Additive.Type.CONDIMENT:
                {
                    if (additivesList1[1].Index == additive.Index)
                    {
                        ticks1[1].sprite = tick;

                    }
                    else
                    {
                        ticks1[1].sprite = cross;
                    }

                    Color color = ticks1[1].color;
                    color.a = 0.5f;
                    ticks1[1].color = color;

                    break;
                }
            case Additive.Type.MILK:
                {
                    if (additivesList1[2].Index == additive.Index)
                    {
                        ticks1[2].sprite = tick;

                    }
                    else
                    {
                        ticks1[2].sprite = cross;
                    }

                    Color color = ticks1[2].color;
                    color.a = 0.5f;
                    ticks1[2].color = color;

                    break;
                }
            default:
                break;
        }

        return;
    }

    public void ClearAdditive(int docket)
    {
        foreach (Image icon in additives1)
        {
            icon.sprite = noAdditive;
        }

        additivesList1.Clear();
        return;
    }
}