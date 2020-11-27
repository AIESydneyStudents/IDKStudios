using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipUI : MonoBehaviour
{
    public Text tipText;

    public void UpdateTip()
    {
        switch (GameEventManager.Instance.makingProgress.Peek())
        {
            case GameEventManager.TeaStage.FILL_KETTLE:
                {
                    tipText.text = "Fill the kettle at the tap and boil it to the desired temperature.";
                    break;
                }
            case GameEventManager.TeaStage.FILL_TEAPOT:
                {
                    tipText.text = "Fill the teapot with the boiled water.";
                    break;
                }
            case GameEventManager.TeaStage.ADD_TEA:
                {
                    tipText.text = "Add tea to the teapot. They can be found in the coloured tea caddies.";
                    break;
                }
            case GameEventManager.TeaStage.FILL_CUP:
                {
                    tipText.text = "Fill the cup by picking up the teapot and clicking the cup with it to transfer tea.";
                    break;
                }
            case GameEventManager.TeaStage.ADD_CONDIMENT:
                {
                    tipText.text = "Add condiments to your tea by clicking on the cinamon or sugar, and clicking on the cup with them.";
                    break;
                }
            case GameEventManager.TeaStage.ADD_MILK:
                {
                    tipText.text = "Add milk to the tea by click on the fridge, selecting the milk type you want, and clicking the cup with it.";
                    break;
                }
            case GameEventManager.TeaStage.SERVE:
                {
                    tipText.text = "Serve the tea to the customer!";
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}
