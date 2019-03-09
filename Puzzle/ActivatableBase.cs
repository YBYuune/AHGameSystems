using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActivatableBase : BaseComponent, Activatable
{ 
    // bool representing whether or not the activatable object is active
    [SerializeField]
    private bool activated;
    public bool Activated
    {
        get
        {
            return activated;
        }
        protected set
        {
            activated = value;
        }
    }
}
