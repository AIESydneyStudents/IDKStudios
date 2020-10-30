using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServeTea : MonoBehaviour
{
    [SerializeField]
    public GameObject servedUI;
    private GameObject Teacup;

    // Start is called before the first frame update
    void Start()
    {
        servedUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Teacup == null)
        {
            servedUI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Saucer")
        {
            Teacup = collider.gameObject;

            // TEMPORARY TEXT APPEARS TO INDICATE ACTION

            servedUI.SetActive(true);

            // WILL ADD ANIMATION HERE

            Destroy(Teacup, 1.0f);
        }
    }
}
