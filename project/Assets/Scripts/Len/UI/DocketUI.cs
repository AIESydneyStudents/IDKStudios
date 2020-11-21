using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DocketUI : MonoBehaviour
{
    public GameObject docket1;
    public GameObject docket2;

    //NEED BARS AND PICTURE FIELDS
    public void ShowDockets()
    {
        gameObject.SetActive(true);

        docket1.SetActive(true);
        //Set fields

        if (GameEventManager.Instance.order2 != null)
        {
            docket2.SetActive(true);
            //Set fields
        }
    }
}
