using CustomEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

[CreateAssetMenu(
    fileName = "New Additive",
    menuName = "Additive",
    order = 0
    )]
public class Additive : ScriptableObject, IComparable<Additive>
{
    #region Fields

    public enum Type
    {
        TEA,
        CONDIMENT
    }

    // Additive dictionary: AdditiveName|Additive
    private static Dictionary<string, Additive> additiveLookup =
        new Dictionary<string, Additive>();

    [Header("Additive Properties")]

    [Tooltip("Additive name. Additive.GetAdditive uses this " +
             "field to find this additive")]
    [SerializeField]
    private string additiveName;

    // Index for this additive in additiveLookup.
    private int additiveIndex;

    [Tooltip("What sort of additive is this?")]
    public Type additiveType;

    [Tooltip("What container can this additive be put into?")]
    public Container.Type container;

    [SerializeField]
    public AttributeModifier initialEffect;

    [Tooltip("When this additive is in a teapot with water, this is how " +
             "much the attributes will change each second.")]
    [SerializeField]
    public AttributeModifier steepEffect;

    [Tooltip("Does this additive require tea to be added first?")]
    public bool teaRequirement;

    [Tooltip("Sets additive as invisible ingredient to docket.")]
    [SerializeField]
    private bool hideFromDocket;

    #endregion

    #region Properties

    // Gets the index of this additive.
    public int Index { get { return additiveIndex; } }

    // Gets the count of all loaded additives.
    public static int AdditiveCount { get { return additiveLookup.Count; } }

    #endregion

    #region Functions

    // Runs at game start. Loads assets into RAM and sets them up.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    public static void InitializeAll()
    {
        // Loads all additives in Resources folder.
        Additive[] additives =
            Resources.LoadAll<Additive>("Common/GameData/Additives/");

        // Adds all loaded additives to static additiveList.
        foreach (Additive additive in additives)
        {
            Additive newAdditive = Instantiate(additive);
            // Additive is added to the lookup by it's name.
            additiveLookup.Add(additive.additiveName, newAdditive);

            // Additive's index is set.
            newAdditive.additiveIndex = additiveLookup.Count - 1;
        }
    }

    public static Additive GetAdditive(string name)
    {
        return additiveLookup.TryGetValue(name, out Additive result) ?
            result : null;
    }

    // Get Additive by index. Returns null if index is out of range.
    public static Additive GetAdditive(int index)
    {
        return additiveLookup.Count > index ?
            additiveLookup.ElementAt(index).Value : null;
    }

    // Get an array of all additives loaded.
    public static Additive[] GetAllAdditives()
    {
        return additiveLookup.Values.ToArray();
    }

    int IComparable<Additive>.CompareTo(Additive other)
    {
        return additiveIndex.CompareTo(other.additiveIndex);
    }

    #endregion
}