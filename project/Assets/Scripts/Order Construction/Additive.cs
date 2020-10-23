using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(
    fileName = "New Additive", 
    menuName = "Additive", 
    order = 0
    )]
public class Additive : ScriptableObject
{
    #region Fields

    // Additive dictionary: AdditiveName|Additive
    private static Dictionary<string, Additive> additiveLookup = 
        new Dictionary<string, Additive>();

    // These are the properties of the additive.
    [Header("Additive Properties")]
    
    [Tooltip("Additive name. Additive.GetAdditive uses this " +
             "field to find this additive")]
    [SerializeField]
    private string additiveName;

    private int additiveIndex;

    private int additiveCountMax;

    [SerializeField]
    public AttributeModifier effectProfile;

    [Tooltip("List of additives that need to be part of " +
             "the order before this additive can be added")]
    public AdditivePrerequisite[] additivePrerequisites;

    [Tooltip("Attributes that order profile needs before " +
             "this additive can be added")]
    public AttributePrerequisite attributePrerequisite;

    [Tooltip("Sets additive to affect order attributes without " +
             "being listed as an ingredient in the order")]
    public bool useEffectOnly;

    [Tooltip("Sets additive as visible ingredient on docket. Does " +
             "nothing if useEffectOnly is set to true")]
    public bool exposeIngredient;

    #endregion

    #region Properties

    // Gets the index of this additive.
    public int Index { get { return additiveIndex; } }

    // Gets the count of all loaded additives.
    public static int AdditiveCount { get { return additiveLookup.Count; } }

    // Gets the maximum amount of this additive that can be
    // contained in an order profile.
    public int AdditiveCountMax { get { return additiveCountMax; } }

    #endregion

    #region Functions

    // Runs at game start. Loads assets into RAM and sets them up.
    [RuntimeInitializeOnLoadMethod(
        RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    private static void InitializeAll()
    {
        // Loads all additives in Resources folder.
        Additive[] additives =
            Resources.LoadAll<Additive>("Common/GameData/Additives/");

        // Adds all loaded additives to static additiveList.
        foreach (Additive additive in additives)
        {
            // Additive is added to the lookup by it's name.
            additiveLookup.Add(additive.additiveName, additive);

            // Additive's index is set.
            additive.additiveIndex = additiveLookup.Count - 1;
        }

        // Every loaded additive is initialized.
        foreach (var additivePair in additiveLookup)
        {
            additivePair.Value.Initialize();
        }
    }

    // Sets up fields.
    public void Initialize()
    {
        // Sort list of additive prerequisites by index.
        Array.Sort(additivePrerequisites);

        // Each additive prerequisite in the additive's prerequisite 
        // list is initialized.
        foreach (AdditivePrerequisite prerequisite in additivePrerequisites)
        {
            prerequisite.Initialize();
        }
    }

    // Get Additive by name. Returns null if additive doesn't exist.
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

    #endregion
}