using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SimpleTrigger))]
public class SimpleSwitch : Interactable
{
    protected SimpleTrigger simpleTriggerComp;

    protected void Start()
    {
        simpleTriggerComp = GetComponent<SimpleTrigger>();

        if (simpleTriggerComp == null)
            Debug.LogError("This component requires a trigger to function. Please attach a SimpleTrigger component to this Game Object");
    }

    // Basic Interact
    public override void Interact(PlayerInteractor player)
    {
        simpleTriggerComp.Toggle();
    }
}
