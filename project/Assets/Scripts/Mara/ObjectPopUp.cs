using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPopUp : MonoBehaviour
{
    private int count = 0;

    [SerializeField]
    public GameObject objectMenu;

    void Start()
    {
        objectMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            objectMenu.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        if (count == 0)
        {
            objectMenu.SetActive(true);
            count++;
        }

        else if (count > 0)
        {
            count = 0;
        }
    }
}
