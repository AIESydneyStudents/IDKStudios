using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeDisplayController : MonoBehaviour
{
    public Text timeDisplayField;

    private void Update()
    {
        if (GameEventManager.Instance.openCustomerTimer == null)
        {
            timeDisplayField.text = "0";
        }
        else
        {
            int elapsedTime = (int)GameEventManager.Instance.openCustomerTimer.ElapsedTime();
            timeDisplayField.text = elapsedTime.ToString();
        }
    }
}
