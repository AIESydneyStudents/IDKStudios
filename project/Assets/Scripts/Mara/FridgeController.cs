using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeController : MonoBehaviour
{
    [SerializeField]
    public GameObject fridge;
    [SerializeField]
    public GameObject creamMilk;
    [SerializeField]
    public GameObject almondMilk;
    private GameObject selectedMilk;

    public void CreamMilk()
    {
        selectedMilk = creamMilk;

        fridge.GetComponent<SpawnObject>().objToSpawn = selectedMilk;
    }

    public void AlmondMilk()
    {
        selectedMilk = almondMilk;

        fridge.GetComponent<SpawnObject>().objToSpawn = selectedMilk;
    }
}
