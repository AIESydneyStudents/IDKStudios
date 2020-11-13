using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public Collider interactionTrigger;
    public bool isObjectSelected;

    public enum ObjectType
    {
        ADDITIVE,
        CONTAINER
    }

    void Start()
    {
        interactionTrigger = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
