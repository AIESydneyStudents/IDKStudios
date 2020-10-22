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

    private int index;

    [Tooltip("List of additives that need to be part of " +
             "the order before this additive can be added")]
    [SerializeField]
    public AdditivePrerequisite[] prerequisites;

    [Tooltip("Sets additive to affect order attributes without " +
             "being listed as an ingredient in the order")]
    [SerializeField]
    private bool useEffectOnly;

    [Tooltip("Sets additive as visible ingredient on docket. Does " +
             "nothing if useEffectOnly is set to true")]
    [SerializeField]
    private bool isVisibleIngredient;

    [Tooltip("Sets additive as reversible. Example: Sugar can not " +
             "be removed. Heat can be removed (cooling down)")]
    [SerializeField]
    private bool canBeRemoved;

    [SerializeField]
    private AttributeModifier effectProfile;

    #endregion

    #region Properties

    public int Index { get { return index; } }

    public static int AdditiveCount { get { return additiveLookup.Count; } }

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
            additive.index = additiveLookup.Count - 1;
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
        // Each additive prerequisite in the additive's prerequisite 
        // list is initialized.
        foreach (AdditivePrerequisite prerequisite in prerequisites)
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