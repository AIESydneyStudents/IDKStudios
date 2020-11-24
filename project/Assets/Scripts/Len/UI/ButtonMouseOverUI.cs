using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMouseOverUI : MonoBehaviour
{
    public RectTransform thisButtonRect;
    public Image buttonImage;
    public MenuController thisMenuController;

    public float x;
    public float y;
    public float width;
    public float height;
    public Vector2 mousePos;

    public void Update()
    {
        x = thisButtonRect.position.x;
        y = thisButtonRect.position.y;
        width = thisButtonRect.rect.width;
        height = thisButtonRect.rect.height;
        mousePos = Input.mousePosition;

        if (mousePos.x > x && mousePos.x < x + width && mousePos.y > y && mousePos.y < y + height)
        {
            buttonImage.color = Color.red;
        }
        else
        {
            buttonImage.color = Color.green;
        }
    }
}
