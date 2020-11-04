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

    [Header("Collective Attributes")]

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float collectiveTaste;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float collectiveStrength;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float collectiveTemperature;

    private List<AdditiveStack> additiveRepository =
        new List<AdditiveStack>();

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
    public bool InsertAdditive(ref AdditiveStack additive,
           bool applyEffect = true,
           bool ignoreAdditivePrerequisites = false,
           bool ignoreAttributePrerequisite = false,
           bool ignoreCollectivePrerequisite = false)
    {
        // Used to keep track of if there is an additive stack
        // with the same index already in the repository and
        // where it is.
        int foundIndex = -1;

        // Check if additive can be added to the order repository.
        if (!PrerequisitesMet(additive,
                               ref foundIndex,
                               ignoreAdditivePrerequisites,
                               ignoreAttributePrerequisite,
                               ignoreCollectivePrerequisite))
        {
            // If not, exit function, additive can not be added.
            return false;
        }

        // If additive is effect only, exit function here.
        if (additive.Additive.useEffectOnly)
        {
            // If apply effect is enabled only.
            if (applyEffect)
            {
                // Apply additiveStack attributes.
                ApplyModifierFromStack(additive);
            }
            
            return true;
        }

        // Check if additive is already part of repository. If found index
        // remains -1, additive stack is not a part of repository.
        if (foundIndex == -1)
        {
            // Used to keep track of greatest insertion position.
            int beforeIndex = -1;

            // Check if additiveRepository is empty. If so, add.
            if (additiveRepository.Count == 0)
            {
                AdditiveStack newStack = new AdditiveStack(additive.index, 0);

                newStack.count = Math.Min(
                    additive.Additive.GetMaxCount(this),
                    additive.count);
                additive.count -= newStack.count;

                additiveRepository.Insert(0, newStack);
            }

            // If not, needs to be inserted in correct location to 
            // maintain ordering.
            else
            {
                for (int i = 0; i < additiveRepository.Count; i++)
                {
                    // If index after beforeIndex is still less than the additive's
                    // index, move beforeIndex up to new index.
                    if (additiveRepository[beforeIndex + 1].index < additive.index)
                    {
                        beforeIndex++;
                    }

                    // If index after beforeIndex is greater than additive's index,
                    // insert here.
                    else
                    {
                        AdditiveStack newStack = new AdditiveStack(additive.index, 0);

                        newStack.count = Math.Min(
                            additive.Additive.GetMaxCount(this),
                            additive.count);
                        additive.count -= newStack.count;

                        additiveRepository.Insert(beforeIndex + 1, newStack);
                    }
                }
            }
        }
        else
        {
            // Insert at the found index.
            AdditiveStack stack = additiveRepository[foundIndex];

            // Store max count for additive.
            int additiveCountMax = stack.Additive.GetMaxCount(this);

            // Check if there's space at all, return if not.
            if (stack.count == additiveCountMax)
            {
                return false;
            }

            // Find left over space in the pre-existing additive stack.
            int leftoverSpace = additiveCountMax - stack.count;

            // Transfer highest possible amount.
            if (leftoverSpace >= additive.count)
            {
                // If apply effect is enabled only.
                if (applyEffect)
                {
                    // Apply all of stack modifier.
                    ApplyModifierFromStack(additive);
                }

                stack.count += additive.count;
                additive.count = 0;
            }
            else
            {
                // If apply effect is enabled only.
                if (applyEffect)
                {
                    // Apply partial amount of stack modifier.
                    ApplyModifierFromStack(additive, leftoverSpace);
                }

                stack.count = additiveCountMax;
                additive.count -= leftoverSpace;
            }
        }

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
            InsertAdditive(ref stack, false);
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
            if (stack.Additive.isVolumetric)
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
