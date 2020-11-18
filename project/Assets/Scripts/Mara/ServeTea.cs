using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServeTea : MonoBehaviour
{
    [SerializeField]
    public GameObject servedUI;
    private GameObject teacup;

    // Start is called before the first frame update
    void Start()
    {
        servedUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (teacup == null)
        {
            servedUI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Saucer")
        {
            teacup = collider.gameObject;

            // TEMPORARY TEXT APPEARS TO INDICATE ACTION

            servedUI.SetActive(true);

            // WILL ADD ANIMATION HERE

            Destroy(teacup, 1.0f);
        }
    }
}
