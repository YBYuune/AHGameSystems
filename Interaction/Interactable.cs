
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : BaseComponent
{
    private static int incrementorID_ = 0;

    // ID that relates to each unique Interactable
    private int id;
    public int InteractableID
    {
        get
        {
            return id;
        }
    }

    // Assign unique id to interactable
    protected void Awake()
    {
        id = incrementorID_++;
    }

    // Basic Interact
    public virtual void Interact(Interactor interactor)
    {
        interactor.Interact(this);
    }

    // Overloaded so that items can have player specific interactions
    public virtual void Interact(PlayerInteractor player)
    {
        player.Interact(this);
    }
}
