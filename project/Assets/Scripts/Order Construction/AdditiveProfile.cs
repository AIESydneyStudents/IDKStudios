using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AdditiveProfile
{
    #region Fields

    private List<AdditiveStack> additiveRepository =
        new List<AdditiveStack>();

    #endregion

    #region Properties

    #endregion

    #region Functions

    // Inserts AdditiveStack into Additive Profile. Returns true if success,
    // returns false if prerequisites not met. If additive is already
    // part of repository, count will be incremented instead.
    public bool InsertAdditive(AdditiveStack additive)
    {
        // Get additive prerequisites of given additive.
        AdditivePrerequisite[] additivePrerequisites =
            additive.Additive.prerequisites;

        // Create a checklist to keep track of fulfillment
        PrerequisiteChecklist checklist = new PrerequisiteChecklist();

        // Used to keep track of if there is an additive stack
        // with the same index already in the repository and
        // where it is.
        int foundIndex = -1;

        // Scan through all additive stacks in additive repository
        // to check for compatibility.
        for (int i = 0; i < additiveRepository.Count; i++)
        {
            AdditiveStack stack = additiveRepository[i];

            if (stack.Index == additive.Index)
            {
                foundIndex = i;
            }

            //NEEDS WORK
        }

        return false;
    }

    #endregion
}
