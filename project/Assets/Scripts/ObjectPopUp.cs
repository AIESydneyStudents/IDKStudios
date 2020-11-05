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
        Time.timeScale = 1.0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            objectMenu.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }

    private void OnMouseDown()
    {
        objectMenu.SetActive(true);
        Time.timeScale = 0.0f;
    }
}
