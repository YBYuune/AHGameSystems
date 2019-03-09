using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleActor : ActivatableBase
{
    // bool representing whether or not the actor can be deactivated once activated
    [SerializeField]
    protected bool activatesOnce;
    public bool SetActivatesOnce
    {
        set
        {
            activatesOnce = value;
        }
    }


    [SerializeField]
    protected List<ActivatableBase> activatableList = new List<ActivatableBase>();


    public override void BaseUpdate()
    {
        base.BaseUpdate();

        UpdateActiveState();
    }

    // Iterates through all linked triggers and checks if they're all active
    // If all triggers are active, then activate this actor
    public virtual void UpdateActiveState()
    {
        // immediately return true if the actor can only activate once and has already been activated
        if (activatesOnce && Activated)
            return;

        bool allActivatablesActive = true;
        foreach(Activatable activatable in activatableList)
        {
            if (activatable != null && !activatable.Activated)
            {
                allActivatablesActive = false;
                break;
            }
        }

        // Active state is dependent on all triggers being active
        Activated = allActivatablesActive;
    }
}
