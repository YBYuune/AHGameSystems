using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OptionsBase
{
    protected bool optionsHaveChanged = false;
    public bool OptionsHaveChanged
    {
        get { return optionsHaveChanged; }
    }

    protected void UpdateChangeStatus<T>(ref T prevValue, T newValue)
    {
        if (!prevValue.Equals(newValue))
        {
            if (!optionsHaveChanged)
                optionsHaveChanged = true;
        }

        prevValue = newValue;
    }

    public abstract void ApplySettings();
}
