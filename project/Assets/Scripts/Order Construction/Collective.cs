using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(
    fileName = "New Collective",
    menuName = "Collective",
    order = 1
    )]
public class Collective : ScriptableObject
{
    #region Fields

    // Collective dictionary: CollectiveName|Collective
    private static Dictionary<string, Collective> collectiveLookup =
        new Dictionary<string, Collective>();

    [Tooltip("Collective name. Collective.GetCollective uses this " +
             "field to find this collective")]
    [SerializeField]
    private string collectiveName;

    // Index for this collective in collectiveLookup.
    private int collectiveIndex;

    [SerializeField]
    [Tooltip("Allows collective to contain water. Water will 'consume' " +
             "additives in the additive repository and move them to the " +
             "graveyard.")]
    private bool isWet;

    [Range(0, 10)]
    [SerializeField]
    [Tooltip("This is how much water is currently in this " +
             "collective")]
    private int waterCount;

    [Range(0, 10)]
    [SerializeField]
    [Tooltip("This is how much water can be contained in this " +
             "collective")]
    private int maxWaterCount;

    [Header("Collective Attributes")]

    [Range(-1.0f, 1.0f)]
    [SerializeField]
    private float collectiveTaste;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float collectiveStrength;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float collectiveTemperature;

    // These are the additives with attribute modifiers yet to be applied.
    private List<AdditiveStack> additiveRepository =
        new List<AdditiveStack>();

    // These are the additives which have been consumed by the addition
    // of water.
    private List<AdditiveStack> additiveGraveyard =
        new List<AdditiveStack>();

    [Header("Whitelist")]

    [Tooltip("Collectives that this collective can be merged " +
             "with")]
    public CollectiveWhitelistEntry[] collectiveWhitelist;

    #endregion

    #region Properties

    // Gets the index of this collective.
    public int Index { get { return collectiveIndex; } }

    public float Taste { get { return collectiveTaste; } }

    public float Strength { get { return collectiveStrength; } }

    public float Temperature { get { return collectiveTemperature; } }
    #endregion

    #region Functions

    // Runs at game start. Loads assets into RAM and sets them up.
    public static void InitializeAll()
    {
        // Loads all collectives in Resources folder.
        Collective[] collectives =
            Resources.LoadAll<Collective>("Common/GameData/Collectives/");

        // Adds all loaded additives to static additiveList.
        foreach (Collective collective in collectives)
        {
            Collective newCollective = Instantiate(collective);

            // Additive is added to the lookup by it's name.
            collectiveLookup.Add(collective.collectiveName, newCollective);

            // Additive's index is set.
            newCollective.collectiveIndex = collectiveLookup.Count - 1;
        }

        foreach (Collective collective in collectives)
        {
            collective.Initialize();
        }
    }

    // Sets up fields.
    private void Initialize()
    {
        // Each whitelist entry is added to the collectiveIndexWhitelist.
        foreach (CollectiveWhitelistEntry entry in collectiveWhitelist)
        {
            // Find index and insert into int List.
            entry.Initialize();
        }
    }

    // Inserts AdditiveStack into Additive Profile. Returns true if success,
    // returns false if prerequisites not met. If additive is already
    // part of repository, count will be incremented instead.
    public bool InsertAdditive(ref AdditiveStack additive)
    {
        // Inserting additives to collective will first check that the collective is allowed to
        // accept the additive. This includes having space for it even if it is accepted.

        // Once it is accepted, it will be consumed if there is water in the collective.
        // Once consumed, it will apply it's attribute modifier which will be inversely proportional
        // to the amount of water in the collective. For example, applying a tea to a collective
        // with two water will make it apply a 0.5 * modifier to the modifier.

        // After an additive has been consumed, it will be added to an additive graveyard IF it isn't
        // effectOnly. If it is effectOnly, it'll disappear.
        
        // Merging two collectives will combine their attributes based on how much water they contribute
        // to the merge result. Partial amounts of water contribution will have the same attributes as 
        // the source. It's just less water.

        // When a merge occurs, if additives in the graveyard are isPersistant, they will duplicate
        // into the mergee's graveyard.



        return false;
    }

    // Checks whether or not you can merge a collective with this
    // collective.
    public bool CanMerge(Collective collective)
    {
        return false;
    }

    // Will merge one collective with another, leaving left leftovers.
    public void MergeCollective(Collective collective)
    {
        List<AdditiveStack> thisCollective = GetAllVolumetric();
        List<AdditiveStack> thatCollective = collective.GetAllVolumetric();
        int thisVolume = 0;
        int thatVolume = 0;
        float totalVolumeInv = 0;

        // Find total volume of this collective.
        foreach(AdditiveStack stack in thisCollective)
        {
            thisVolume += stack.count;
        }

        // Find total volume of that collective.
        foreach (AdditiveStack stack in thatCollective)
        {
            thatVolume += stack.count;
        }

        // Find total volume of both collectives.
        totalVolumeInv = 1.0f / (thisVolume + thatVolume);

        collectiveTaste = Math.Min(Taste + collective.Taste, 1);
        collectiveStrength = Math.Min(Strength + collective.Strength, 1);
        collectiveTemperature = Math.Min(Temperature * (thisVolume * totalVolumeInv)
            + collective.Temperature * (thatVolume * totalVolumeInv), 1);

        // For each additive stack in collective, insert into this
        // collective.
        for (int i = 0; i < collective.additiveRepository.Count; i++)
        {
            AdditiveStack stack = collective.additiveRepository[i];
            //InsertAdditive(ref stack, false);
        }

        // NEED TO FIGURE OUT HOW TO MERGE ATTRIBUTES
        // assemble a list of volumetric additives in both collectives. Use count as weighting
        // for adding attributes.

    }

    public bool Empty()
    {
        return additiveRepository.Count == 0;
    }

    private bool PrerequisitesMet(AdditiveStack additive,
                               ref int sameAdditiveIndex,
                               bool ignoreAdditivePrerequisites = false,
                               bool ignoreAttributePrerequisite = false,
                               bool ignoreCollectivePrerequisite = false)
    {
        return
            ignoreAdditivePrerequisites  ? true : AdditivePrerequisitesMet(additive, ref sameAdditiveIndex) &&
            ignoreAttributePrerequisite  ? true : AttributePrerequisiteMet(additive) &&
            ignoreCollectivePrerequisite ? true : CollectivePrerequisiteMet(additive);
    }

    private bool AdditivePrerequisitesMet(AdditiveStack additive, ref int sameAdditiveIndex)
    {
        // Initalize same additive index.
        sameAdditiveIndex = -1;

        // Get additivePrerequisite array.
        AdditivePrerequisite[] additivePrerequisites = 
            additive.Additive.additivePrerequisites;

        // For every prerequisite, scan the repository for the additive.
        // Keep track of current repository position. This will prevent
        // repeated scans of entire list by exploiting the fact that
        // the repository and the prerequisite array are ordered by index.
        int currentRepoIndex = 0;

        // Scan through for each prerequisite.
        for (int i = 0; i < additivePrerequisites.Length; i++)
        {
            // Grab the currentPrerequisite to be checked.
            AdditivePrerequisite currentPrerequisite = additivePrerequisites[i];

            // Grab the index as well.
            int currentPrerequisiteIndex = currentPrerequisite.Index;

            // Starting at the current repository index, keep iterating through
            // until either index is found, or index is superseded.
            while (true)
            {
                // Check to see if the currently examined repository stack matches 
                // the additive we are checking for.
                if (additiveRepository[currentRepoIndex].index == additive.index)
                {
                    sameAdditiveIndex = currentRepoIndex;
                }

                // If this repository index is less than the desired prerequisite
                // index, increment repository position.
                if (additiveRepository[currentRepoIndex].index < currentPrerequisiteIndex)
                {
                    currentRepoIndex++;

                    continue;
                }

                // If this repository index is equal to the desired prerequisite
                // index, further investigate it's count.
                if (additiveRepository[currentRepoIndex].index == currentPrerequisiteIndex)
                {
                    // Check if the pre-set count constraints are met by the found
                    // prerequisite.
                    bool constraintsMet = currentPrerequisite.IsCountValid(
                        additiveRepository[currentRepoIndex].count);

                    // If count is valid, break this check, and move onto next
                    // prerequisite.
                    if (constraintsMet)
                    {
                        break;
                    }

                    // If count is not valid, exit function, no need to
                    // continue cross checking.
                    else
                    {
                        return false;
                    }
                }

                // If the current repository additive's index is greater than 
                // desired, exit function, no need to continue cross checking.
                if (additiveRepository[currentRepoIndex].index > currentPrerequisiteIndex)
                {
                    return false;
                }
            }
        }

        // If this point is reached, entire check was conducted, and all
        // prerequisites are met.
        return true;
    }

    private bool AttributePrerequisiteMet(AdditiveStack additive)
    {
        return additive.Additive.attributePrerequisite.IsTasteValid(collectiveTaste) &&
               additive.Additive.attributePrerequisite.IsStrengthValid(collectiveStrength) &&
               additive.Additive.attributePrerequisite.IsTemperatureValid(collectiveTemperature);
    }

    private bool CollectivePrerequisiteMet(AdditiveStack additive)
    {
        return additive.Additive.InWhitelist(collectiveIndex);
    }

    private void ApplyModifierFromStack(AdditiveStack stack)
    {
        collectiveTaste       = Math.Max(Math.Min(collectiveTaste +       stack.count * stack.Additive.effectProfile.Taste,       1.0f), 0.0f);
        collectiveStrength    = Math.Max(Math.Min(collectiveStrength +    stack.count * stack.Additive.effectProfile.Strength,    1.0f), 0.0f);
        collectiveTemperature = Math.Max(Math.Min(collectiveTemperature + stack.count * stack.Additive.effectProfile.Temperature, 1.0f), 0.0f);
    }

    private void ApplyModifierFromStack(AdditiveStack stack, int amount)
    {
        if (amount < 1)
        {
            return;
        }

        amount = Math.Min(stack.count, amount);

        collectiveTaste       = Math.Max(Math.Min(collectiveTaste +       amount * stack.Additive.effectProfile.Taste,       1.0f), 0.0f);
        collectiveStrength    = Math.Max(Math.Min(collectiveStrength +    amount * stack.Additive.effectProfile.Strength,    1.0f), 0.0f);
        collectiveTemperature = Math.Max(Math.Min(collectiveTemperature + amount * stack.Additive.effectProfile.Temperature, 1.0f), 0.0f);
    }

    // Get all volumetric additives in this collective.
    public List<AdditiveStack> GetAllVolumetric()
    {
        List<AdditiveStack> volumetricAdditives = new List<AdditiveStack>();

        foreach (AdditiveStack stack in additiveRepository)
        {
            //if (stack.Additive.isVolumetric)
            {
                volumetricAdditives.Add(stack);
            }
        }

        return volumetricAdditives;
    }

    // Get Collective by name. Returns null if collective doesn't exist.
    public static Collective GetCollective(string name)
    {
        return collectiveLookup.TryGetValue(name, out Collective result) ?
            result : null;
    }

    // Get Collective by index. Returns null if index is out of range.
    public static Collective GetCollective(int index)
    {
        return collectiveLookup.Count > index ?
            collectiveLookup.ElementAt(index).Value : null;
    }

    // Get an array of all additives loaded.
    public static Collective[] GetAllCollectives()
    {
        return collectiveLookup.Values.ToArray();
    }

    #endregion
}
