using System.Collections.Generic;
using UnityEngine;

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
    
    [Tooltip("Additive name. Additive.GetAdditive uses this" +
             "field to find this additive")]
    [SerializeField]
    private string additiveName;

    [Tooltip("List of additives that need to be part of" +
             "the order before this additive can be added")]
    [SerializeField]
    private string[] additivePrerequisites;
    private Additive[] prerequisites;

    [Tooltip("Determines whether or not it will be added as" +
             "a persistant ingredient that will also show up" +
             "on the order docket")]
    [SerializeField]
    private bool isVisibleIngredient;

    // The specific attributes.
    [Header("Additive Attributes")]

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float effectTaste;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float effectFlavour;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float effectTemperature;

    private TeaProfile effectProfile;

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
        prerequisites = new Additive[additivePrerequisites.Length];

        // Each additive in the additive's prerequisite list is
        // found and added to the prerequisites.
        for (int i = 0; i < additivePrerequisites.Length; i++)
        {
            string thisAdditiveName = additivePrerequisites[i];
            prerequisites[i] = additiveList[thisAdditiveName];
        }

        effectProfile = new TeaProfile(
            effectTaste, 
            effectFlavour, 
            effectTemperature);
    }

    #endregion
}