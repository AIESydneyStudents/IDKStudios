using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DocketUIController : MonoBehaviour
{
    public Text targetTaste;
    public Text targetStrength;
    public Text targetTemperature;
    public Text addedIngredient;

    public void SetDocket(Order order)
    {
        SetAttributes(order.targetTaste, order.targetStrength, order.targetTemperature);
        SetIngredients(order.additiveRepository);
    }

    private void SetAttributes(float taste, float strength, float temperature)
    {
        targetTaste.text = taste.ToString();
        targetStrength.text = strength.ToString();
        targetTemperature.text = temperature.ToString();
    }

    private void SetIngredients(SortedSet<Additive> additiveRepository)
    {
        if (additiveRepository == null)
        {
            return;
        }

        int i = 0;

        foreach (Additive additive in additiveRepository)
        {
            if (i == 0)
            {
                addedIngredient.text = additive.Name;
                i++;
            }
            else
            {
                Instantiate(addedIngredient, addedIngredient.transform.parent.transform);
            }
        }
    }
}
