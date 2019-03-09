using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTrigger : ActivatableBase
{
    public virtual bool Deactivate()
    {
        Activated = false;

        return Activated;
    }

    public virtual bool Activate()
    {
        Activated = true;

        return Activated;
    }

    public virtual bool Toggle()
    {
        Activated = !Activated;

        return Activated;
    }
}
