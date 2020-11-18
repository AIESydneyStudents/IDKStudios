using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerLoader : MonoBehaviour
{
    private enum State
    {
        INACTIVE,
        FADING_IN,
        FADING_OUT
    }

    public MeshFilter customerMeshFilter;
    public MeshRenderer customerMeshRenderer;

    public float fadeOutTime = 1.0f;
    private Customer currentCustomer = null;
    private State state = State.INACTIVE;

    public Customer customer1;
    public Customer customer2;

    public void SetCustomer( Customer customer )
    {
        if (currentCustomer == customer)
        {
            return;
        }

        if (currentCustomer == null)
        {
            currentCustomer = customer;
            customerMeshRenderer.material = currentCustomer.customerMaterial;
            customerMeshFilter.mesh = currentCustomer.customerMesh;
            Alpha = 0.0f;
            state = State.FADING_IN;
            customerMeshRenderer.enabled = true;
        }
        else
        {
            currentCustomer = customer;
            state = State.FADING_OUT;
            customerMeshRenderer.enabled = true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetCustomer(customer1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetCustomer(customer2);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetCustomer(null);

        switch (state)
        {
            case State.FADING_IN:
                {
                    if (Alpha >= 1.0f)
                    {
                        state = State.INACTIVE;
                    }

                    Alpha += Time.deltaTime / fadeOutTime;

                    break;
                }
            case State.FADING_OUT:
                {
                    if (Alpha <= 0.0f)
                    {
                        if (currentCustomer == null)
                        {
                            customerMeshRenderer.enabled = false;
                        }
                        else
                        {
                            customerMeshRenderer.material = currentCustomer.customerMaterial;
                            customerMeshFilter.mesh = currentCustomer.customerMesh;
                            Alpha = 0.0f;
                            state = State.FADING_IN;
                        }
                    }

                    Alpha -= Time.deltaTime / fadeOutTime;

                    break;
                }
            default:
                break;
        }
    }

    private float Alpha
    {
        get
        {
            if (customerMeshRenderer == null ||
                customerMeshRenderer.material == null)
            {
                return 0.0f;
            }

            return customerMeshRenderer.material.GetColor( "_Color" ).a;
        }

        set
        {
            Color color = customerMeshRenderer.material.GetColor( "_Color" );
            
            if (customerMeshRenderer == null ||
                customerMeshRenderer.material == null ||
                Mathf.Approximately( color.a, value))
            {
                return;
            }

            color.a = Mathf.Clamp01(value);
            customerMeshRenderer.material.SetColor( "_Color", color );
        }
    }
}
