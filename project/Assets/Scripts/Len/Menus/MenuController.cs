using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public Vector3 offset;

    public enum MenuType
    {
        KETTLE_MENU,
        FRIDGE_MENU
    }

    public MenuType menuType;

    public void ShowMenu(Vector3 screenPosition)
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition = screenPosition + offset;
        gameObject.SetActive(true);
        InputController.Instance.DisableInteraction();
    }

    public void HideMenu()
    {
        gameObject.SetActive(false);
        InputController.Instance.EnableInteraction();
    }
}
