using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DocketUI : MonoBehaviour
{
    public GameObject docket1;
    public GameObject docket2;

    public Button docket1Button;
    public Button docket2Button;

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

    #region Docket 2 Data
    [Header("Docket 2")]

    public AttributeSliderController tasteSlider2;
    public AttributeSliderController strengthSlider2;
    public AttributeSliderController temperatureSlider2;

    public List<Additive> additivesList2 = new List<Additive>();
    public Image[] additives2;
    public Image[] ticks2;
    public Image capacity2;

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
        ResetDockets();

        gameObject.SetActive(true);

        docket1.SetActive(true);

        if (GameEventManager.Instance.order2 != null)
        {
            docket2.SetActive(true);
        }
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

        #region Docket2 Sliders
        if (GameEventManager.Instance.order2 != null)
        {
            Cup cup2 = GameEventManager.Instance.cupInterface2.cup;

            tasteSlider2.UpdateSlider(
                GameEventManager.Instance.MinTasteAdjusted2,
                GameEventManager.Instance.MaxTasteAdjusted2,
                cup2.Taste);

            strengthSlider2.UpdateSlider(
                GameEventManager.Instance.MinStrengthAdjusted2,
                GameEventManager.Instance.MaxStrengthAdjusted2,
                cup2.Strength);

            temperatureSlider2.UpdateSlider(
                GameEventManager.Instance.MinTemperatureAdjusted2,
                GameEventManager.Instance.MaxTemperatureAdjusted2,
                cup2.Temperature);
        }
        #endregion
    }

    public void HideDocketSubmit()
    {
        docket1Button.gameObject.SetActive(false);
        docket2Button.gameObject.SetActive(false);
    }

    public void ShowDocketSubmit()
    {
        docket1Button.gameObject.SetActive(true);
        docket2Button.gameObject.SetActive(true);
    }

    public void InsertAdditive(Additive additive, int docket)
    {
        switch (docket)
        {
            case 1:
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
            case 2:
                {
                    switch (additive.additiveType)
                    {
                        case Additive.Type.TEA:
                            {
                                if (additivesList2[0].Index == additive.Index)
                                {
                                    ticks2[0].sprite = tick;

                                }
                                else
                                {
                                    ticks2[0].sprite = cross;
                                }

                                Color color = ticks2[0].color;
                                color.a = 0.5f;
                                ticks2[0].color = color;

                                break;
                            }
                        case Additive.Type.CONDIMENT:
                            {
                                if (additivesList2[1].Index == additive.Index)
                                {
                                    ticks2[1].sprite = tick;

                                }
                                else
                                {
                                    ticks2[1].sprite = cross;
                                }

                                Color color = ticks2[1].color;
                                color.a = 0.5f;
                                ticks2[1].color = color;

                                break;
                            }
                        case Additive.Type.MILK:
                            {
                                if (additivesList2[2].Index == additive.Index)
                                {
                                    ticks2[2].sprite = tick;

                                }
                                else
                                {
                                    ticks2[2].sprite = cross;
                                }

                                Color color = ticks2[2].color;
                                color.a = 0.5f;
                                ticks2[2].color = color;

                                break;
                            }
                        default:
                            break;
                    }

                    return;
                }
            default:
                {
                    return;
                }
        }
    }

    public void ClearAdditive(int docket)
    {
        switch (docket)
        {
            case 1:
                {
                    foreach (Image icon in additives1)
                    {
                        icon.sprite = noAdditive;
                    }

                    additivesList1.Clear();
                    return;
                }
            case 2:
                {
                    foreach (Image icon in additives2)
                    {
                        icon.sprite = noAdditive;
                    }

                    additivesList2.Clear();
                    return;
                }
            default:
                {
                    return;
                }
        }
    }

    public void SetDocketCup(int docket, bool fillValue)
    {
        switch (docket)
        {
            case 1:
                {
                    if (fillValue)
                    {
                        capacity1.sprite = cupFull;
                    }
                    else
                    {
                        capacity1.sprite = cupEmpty;
                    }

                    return;
                }
            case 2:
                {
                    if (fillValue)
                    {
                        capacity2.sprite = cupFull;
                    }
                    else
                    {
                        capacity2.sprite = cupEmpty;
                    }

                    return;
                }
            default:
                {
                    return;
                }
        }
    }

    public void ResetDockets()
    {
        additivesList1.Clear();
        additivesList2.Clear();

        tasteSlider1.UpdateSlider(0, 0, -1);
        tasteSlider2.UpdateSlider(0, 0, -1);
        strengthSlider1.UpdateSlider(0, 0, -1);
        strengthSlider2.UpdateSlider(0, 0, -1);
        temperatureSlider1.UpdateSlider(0, 0, -1);
        temperatureSlider2.UpdateSlider(0, 0, -1);

        if (GameEventManager.Instance.order1 != null)
        {
            List<Additive> orderAdditives1 = GameEventManager.Instance.order1.additiveRepository;

            int i = 0;

            foreach (Additive additive in orderAdditives1)
            {
                additives1[i++].sprite = additive.additiveSprite;
                additivesList1.Add(additive);
            }

            foreach (Image sprite in ticks1)
            {
                Color color = sprite.color;
                color.a = 0.0f;
                sprite.color = color;
            }
        }

        if (GameEventManager.Instance.order2 != null)
        {
            List<Additive> orderAdditives2 = GameEventManager.Instance.order2.additiveRepository;

            int j = 0;

            foreach (Additive additive in orderAdditives2)
            {
                additives2[j++].sprite = additive.additiveSprite;
                additivesList2.Add(additive);
            }

            foreach (Image sprite in ticks2)
            {
                Color color = sprite.color;
                color.a = 0.0f;
                sprite.color = color;
            }
        }
    }

    public void TriggerIconUpdate()
    {
        ResetDockets();

        foreach (Additive additive in GameEventManager.Instance.cupInterface1.cup.additiveRepository)
        {
            InsertAdditive(additive, 1);
        }

        foreach (Additive additive in GameEventManager.Instance.cupInterface2.cup.additiveRepository)
        {
            InsertAdditive(additive, 2);
        }

        SetDocketCup(1, GameEventManager.Instance.cupInterface1.cup.IsFull);
        SetDocketCup(2, GameEventManager.Instance.cupInterface2.cup.IsFull);
    }
}