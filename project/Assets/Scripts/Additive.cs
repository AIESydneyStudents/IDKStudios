using System.Collections.Generic;
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
    private static Dictionary<string, Additive> additiveList = 
        new Dictionary<string, Additive>();

    // These are the properties of the additive.
    [Header("Additive Properties")]
    
    [Tooltip("Additive name. Additive.GetAdditive uses this " +
             "field to find this additive")]
    [SerializeField]
    private string additiveName;

    [Tooltip("List of additives that need to be part of " +
             "the order before this additive can be added")]
    [SerializeField]
    private string[] additivePrerequisites;
    private Additive[] prerequisites;

    [Tooltip("Sets additive to affect order attributes without " +
             "being listed as an ingredient in the order")]
    [SerializeField]
    private bool useEffectOnly;

    [Tooltip("Sets additive as visible ingredient on docket. Does " +
             "nothing if useEffectOnly is set to true")]
    [SerializeField]
    private bool isVisibleIngredient;
    private static int visibleIngredientCount;

    [SerializeField]
    private AttributeModifier effectProfile;

    #endregion

    #region Properties

    public static int VisibleIngredientsCount { get { return visibleIngredientCount; } }

    #endregion

    #region Functions

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
            additiveList.Add(additive.additiveName, additive);

            // If additive will appear on docket as visible ingredient,
            // visible ingredient count is incremented.
            if (additive.isVisibleIngredient)
            {
                visibleIngredientCount++;
            }
        }

        // Every loaded additive is initialized.
        foreach (var additivePair in additiveList)
        {
            additivePair.Value.Initialize();
        }
    }

    // Sets up fields.
    public void Initialize()
    {
        // A prerequisite list is made for this additive.
        //prerequisites = new Additive[additivePrerequisites.Length];

        // Each additive in the additive's prerequisite list is
        // found and added to the prerequisites.
        //for (int i = 0; i < additivePrerequisites.Length; i++)
        //{
        //    string thisAdditiveName = additivePrerequisites[i];
        //    prerequisites[i] = additiveList[thisAdditiveName];
        //}

        // Effect profile is initalized with specified attributes.
        //effectProfile = new AttributeModifier(
        //    effectTaste, 
        //    effectFlavour, 
        //    effectTemperature);
    }

    #endregion
}