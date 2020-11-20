using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DocketUIController : MonoBehaviour
{
    public Text targetTaste;
    public Text targetStrength;
    public Text targetTemperature;

    public List<Text> ingredientFields = new List<Text>();
    public Text addedIngredient;

    public void SetDocket(Order order)
    {
        gameObject.SetActive(true);
        SetAttributes(order.targetTaste, order.targetStrength, order.targetTemperature);
        SetIngredients(order.additiveRepository);
    }

    public void HideDocket()
    {
        gameObject.SetActive(false);
    }

    private void SetAttributes(float taste, float strength, float temperature)
    {
        targetTaste.text = taste.ToString();
        targetStrength.text = strength.ToString();
        targetTemperature.text = temperature.ToString();
    }

    private void SetIngredients(SortedSet<Additive> additiveRepository)
    {
        foreach (Text field in ingredientFields)
        {
            Destroy(field);
        }

        if (additiveRepository == null)
        {
            addedIngredient.gameObject.SetActive(false);
            return;
        }

        if (additiveRepository.Count == 0)
        {
            addedIngredient.enabled = false;
            return;
        }

        int i = 0;

        foreach (Additive additive in additiveRepository)
        {
            if (i == 0)
            {
                addedIngredient.gameObject.SetActive(true);
                addedIngredient.text = additive.Name;
                i++;
            }
            else
            {
                Text newIngredient = Instantiate(addedIngredient, addedIngredient.transform.parent.transform);
                newIngredient.text = additive.Name;
                Vector2 position = newIngredient.rectTransform.anchoredPosition;
                position.y -= i * newIngredient.rectTransform.rect.height;
                newIngredient.rectTransform.anchoredPosition = position;

                ingredientFields.Add(newIngredient);
                i++;
            }
        }
    }
}
