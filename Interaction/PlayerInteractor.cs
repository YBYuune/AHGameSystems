using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerInteractor : Interactor
{
    [SerializeField]
    private PlayerController playerController;

    private PlayerInventory playerInventory = null;

    private new void Awake()
    {
        base.Awake();

        // setting up player inventory
        playerInventory = GetComponent<PlayerInventory>();

        if (playerInventory == null)
            playerInventory = GetComponentInParent<PlayerInventory>();

        // Set up player controller
        playerController = GetComponent<PlayerController>();

        if (playerController == null)
            playerController = GetComponentInParent<PlayerController>();

        if (playerController == null)
        {
            Debug.LogError("PlayerInteractor component cannot locate PlayerController on current GameObject or its parent");
        }
    }

    public override void BaseUpdate()
    {
        base.BaseUpdate();

        if (InputManager.GetActionPressed(GameAction.INTERACT) &&
            closestInteractable != null)
        {
            InteractWithObject(closestInteractable);
        }
    }

    private void InteractWithObject(Interactable interactable)
    {
        interactable.Interact(this);
    }

    public bool InteractWithPickup(Pickup pickup)
    {
        if (playerInventory.GetTotalItemCount() < 15)
        {
            // Add item to inventory
            playerInventory.AddItem(pickup);
            return true;
        }

        return false;
    }

    public void InteractWithVent(Vent vent)
    {
        // Get Position of other side of vent
        Vector3? exitPosition = vent.GetOtherExitLocation();

        if (exitPosition.HasValue)
        {
            transform.parent.position = exitPosition.Value;
        }
    }

    public void InteractWithHidingSpot(HidingSpot hidingSpot)
    {
        // If we're not currently hiding
        if (!playerController.IsHiding)
        {
            // Process entering of hiding spot
            EnterHidingSpot();
        }
        else
        {
            // Process exiting of hiding spot
            ExitHidingSpot();
        }
    }

    private void EnterHidingSpot()
    {
        ...
    }

    private void ExitHidingSpot()
    {
		...
    }
}
