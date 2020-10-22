using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class PrerequisiteChecklist
{
    #region Fields

    private bool[] checklist;

    #endregion

    #region Properties

    #endregion

    #region Functions

    public PrerequisiteChecklist()
    {
        // Create bool array same size as total amount of loaded
        // additives.
        checklist = new bool[Additive.AdditiveCount];
    }

    // Checks if checklist at index is fulfilled.
    public bool GetCheck(int index)
    {
        // Out of range check.
        if (index < 0 || index > checklist.Length - 1)
            return false;

        return checklist[index];
    }

    // Gets status of index in checklist.
    public void SetCheck(int index)
    {
        // Out of range check.
        if (index < 0 || index > checklist.Length - 1)
            return;

        checklist[index] = true;
    }

    #endregion
}
