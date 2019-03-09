using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Light))]
public class ActorLight : SimpleActor
{
    private Light lightComp;

	// Use this for initialization
	private void Awake ()
    {
        lightComp = GetComponent<Light>();
        
        if (lightComp == null)
            Debug.LogError("This component requires a Light component to function. Please attach one to this Game Object.");
    }

    // Update is called once per frame
    public override void BaseUpdate()
    {
        base.BaseUpdate();
        
        // Light is on if the actor is activated
        lightComp.enabled = Activated;
    }
}

