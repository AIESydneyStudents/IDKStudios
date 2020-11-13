using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public Camera camera;
    public GameObject heldObject;
    public RaycastHit hit;

    void Start()
    {

    }

    void Update()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit, 10, 0);
    }

    private void OnMouseDown()
    {
        if (hit.collider.gameObject.TryGetComponent(out InteractionController controller))
        {
            heldObject = hit.collider.gameObject;
        }
    }

    private void OnMouseUp()
    {
        
    }
}
