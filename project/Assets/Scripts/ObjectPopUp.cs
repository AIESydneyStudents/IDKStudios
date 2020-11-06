using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPopUp : MonoBehaviour
{
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
        objectMenu.SetActive(true);
    }
}
