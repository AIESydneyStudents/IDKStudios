using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class OrderProfile
{
    #region Fields

    [Header("Current Order Attributes")]

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float currentTaste;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float currentFlavour;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float currentTemperature;

    [Header("Target Order Attributes")]

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float targetTaste;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float targetFlavour;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float targetTemperature;

    [Header("Attribute Tolerances")]

    [SerializeField]
    private float toleranceTaste;

    [SerializeField]
    private float toleranceFlavour;

    [SerializeField]
    private float toleranceTemperature;

    private List<AdditiveStack> additiveRepository =
        new List<AdditiveStack>();

    #endregion

    #region Properties

    #endregion

    #region Functions

    // Inserts AdditiveStack into Additive Profile. Returns true if success,
    // returns false if prerequisites not met. If additive is already
    // part of repository, count will be incremented instead.
    public bool InsertAdditive(AdditiveStack additive,
           bool ignoreAdditivePrerequisites = false,
           bool ignoreAttributePrerequisite = false)
    {
        // Used to keep track of if there is an additive stack
        // with the same index already in the repository and
        // where it is.
        int foundIndex = -1;

        // Check if additive can be added to the order repository.
        if (!PrerequisiteCheck(additive,
                               ref foundIndex,
                               ignoreAdditivePrerequisites,
                               ignoreAttributePrerequisite))
        {
            // If not, exit function, additive can not be added.
            return false;
        }

        // Apply additiveStack attributes.
        ApplyModifierFromStack(additive);

        // If additive is effect only, exit function here.
        if (additive.Additive.useEffectOnly)
        {
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
                additiveRepository.Insert(0, additive);
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
                        additiveRepository.Insert(beforeIndex + 1, additive);
                    }
                }
            }
        }
        else
        {
            // Insert at the found index.
            AdditiveStack stack = additiveRepository[foundIndex];

            // Add counts. Restrain the count to additive count max.
            stack.count = Math.Min(stack.count + additive.count,
                                   additive.Additive.AdditiveCountMax);
        }

        return false;
    }

    private bool PrerequisiteCheck(AdditiveStack additive,
                               ref int sameAdditiveIndex,
                               bool ignoreAdditivePrerequisites = false,
                               bool ignoreAttributePrerequisite = false)
    {
        if (ignoreAdditivePrerequisites && ignoreAttributePrerequisite)
        {
            return true;
        }
        else if (ignoreAdditivePrerequisites)
        {
            return AttributePrerequisiteMet(additive);
        }
        else if (ignoreAttributePrerequisite)
        {
            return AdditivePrerequisitesMet(additive, ref sameAdditiveIndex);
        }

        return AttributePrerequisiteMet(additive) && AdditivePrerequisitesMet(additive, ref sameAdditiveIndex);
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
        return additive.Additive.attributePrerequisite.IsTasteValid(currentTaste) &&
               additive.Additive.attributePrerequisite.IsFlavourValid(currentFlavour) &&
               additive.Additive.attributePrerequisite.IsTemperatureValid(currentTemperature);
    }

    private void ApplyModifierFromStack(AdditiveStack stack)
    {
        currentTaste       = Math.Max(Math.Min(currentTaste +       stack.count * stack.Additive.effectProfile.Taste,       1.0f), 0.0f);
        currentFlavour     = Math.Max(Math.Min(currentFlavour +     stack.count * stack.Additive.effectProfile.Flavour,     1.0f), 0.0f);
        currentTemperature = Math.Max(Math.Min(currentTemperature + stack.count * stack.Additive.effectProfile.Temperature, 1.0f), 0.0f);
    }

    #endregion
}
