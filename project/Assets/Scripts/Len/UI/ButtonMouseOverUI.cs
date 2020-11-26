using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonMouseOverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public IngredientUI ingredientUI;
    public Additive additive;
    public bool stillOverButton;

    private void Update()
    {
        if (stillOverButton)
        {
            ingredientUI.UpdatePosition(Input.mousePosition);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ingredientUI.SetAdditive(additive);
        ingredientUI.ShowUI();

        ingredientUI.UpdatePosition(Input.mousePosition);
        stillOverButton = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ingredientUI.HideUI();
        stillOverButton = false;
    }
}
