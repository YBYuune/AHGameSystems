using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(SphereCollider))]
public abstract class Interactor : BaseComponent
{
    private Interactable baseClosestInteractable;
    protected Interactable closestInteractable
    {
        get
        {
            return baseClosestInteractable;
        }
        set
        {
            // Call overridable function when value is changed
            if (baseClosestInteractable != value)
                OnClosestInteractableChange(baseClosestInteractable, value);

            // Change value
            baseClosestInteractable = value;
        }
    }


    protected Dictionary<int, Interactable> nearbyInteractables;
    private List<int> interactablesToRemove;

    protected void Awake()
    {
        nearbyInteractables = new Dictionary<int, Interactable>();

        interactablesToRemove = new List<int>();
    }

    public override void BaseUpdate()
    {
        UpdateClosestInteractable();
    }

    public virtual void Interact(Interactable interactable) { }

    // Function to be called when the closest interactable changes
    protected virtual void OnClosestInteractableChange(Interactable oldClosest, Interactable newClosest) { }


    private void UpdateClosestInteractable()
    {
        // Make sure we have any interactables
        if (nearbyInteractables.Count == 0)
        {
            closestInteractable = null;
            return;
        }

        Interactable closest = null;
        foreach (int iKey in nearbyInteractables.Keys)
        {
            Interactable interactable = nearbyInteractables[iKey];

            // If the gameObject is inactive (ie Player picked it up)
            // or is null (because it is to be removed)
            if (interactable == null || !interactable.gameObject.activeSelf)
                continue;

            if (closest == null)
            {
                closest = interactable;
            }
            else
            {
                float closestDistanceSquared = (closest.transform.position - transform.position).sqrMagnitude;
                float interactableDistanceSquared = (interactable.transform.position - transform.position).sqrMagnitude;

                if (interactableDistanceSquared < closestDistanceSquared)
                {
                    closest = interactable;
                }
            }
        }

        // Remove after iterating
        int numberOfInteractablesToRemove = interactablesToRemove.Count;
        for (int i = 0; i < numberOfInteractablesToRemove; ++i)
        {
            nearbyInteractables.Remove(interactablesToRemove[i]);
        }

        if (numberOfInteractablesToRemove > 0)
            interactablesToRemove.RemoveRange(0, numberOfInteractablesToRemove);


        // Update closest interactable
        closestInteractable = closest;
    }


    private void OnTriggerEnter(Collider other)
    {
        GameObject otherObject = other.gameObject;
        Interactable interactable = otherObject.GetComponent<Interactable>();

        if (interactable != null && !nearbyInteractables.ContainsKey(interactable.InteractableID))
        {
            nearbyInteractables.Add(interactable.InteractableID, interactable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject otherObject = other.gameObject;
        Interactable interactable = otherObject.GetComponent<Interactable>();

        if (interactable != null && nearbyInteractables.ContainsKey(interactable.InteractableID))
        {
            // Add this item to the list to be removed from the items to be proximity checked
            interactablesToRemove.Add(interactable.InteractableID);

            // set this item to be ignored by the proximity check
            nearbyInteractables[interactable.InteractableID] = null;
        }
    }
}
