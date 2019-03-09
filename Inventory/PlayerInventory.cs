using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerInventory : BaseComponent
{
    // Delegate describes structure of function needed for InventoryChangeEvent
    public delegate void InventoryChange(PlayerInventory playerInventory);

    // Observers can subscribe to event to run a function (that follows the InventoryChange delegate structure)
    // that will be called when the Event is created
    public static event InventoryChange InventoryChangeEvent;


    // DEPENDENCY
    [SerializeField]
    private PlayerController playerController;

    // Actual item inventory
    private ItemInventory itemInventory;


    private List<InventoryItemType> inventoryOrder = new List<InventoryItemType>();
    private int inventoryIndex = 0;

    private void Awake()
    {
        itemInventory = new ItemInventory();

        playerController = GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("PlayerInventory component cannot locate PlayerController on current GameObject");
        }
    }

    public override void BaseUpdate()
    {
        base.BaseUpdate();

        if (playerController.MoveInventoryLeft)
        {
            DecrementIndex();
        }
        else if (playerController.MoveInventoryRight)
        {
            IncrementIndex();
        }
    }

    public List<InventoryItemType> GetInventoryOrder()
    {
        return inventoryOrder;
    }

    public int AddItem(Pickup pickup)
    {
        // Check if the itemType exists in the inventory
        if (!itemInventory.ItemExistsInInventory(pickup.PickupObject.ItemType))
        {
            if (inventoryOrder.Count <= 0)
                inventoryIndex = 0;

            inventoryOrder.Add(pickup.PickupObject.ItemType);
        }

        // Actually add to inventory
        int x = itemInventory.AddItem(pickup);

        Debug.Log("Current count: " + x);

        // Generate inventory change event
        OnInventoryChange();

        return itemInventory.GetItemCount(pickup.PickupObject.ItemType);
    }


    public bool RemoveItem(InventoryItemType itemType)
    {
        return RemoveItem(itemType, 1);
    }

    public bool RemoveItem(InventoryItemType itemType, int numberToRemove)
    {
        // Check if itemType exists in dictionary and we have more than 0 of the given type
        if (itemInventory.RemoveItem(itemType, numberToRemove) >= 0)
        {
            ValidateInventory();

            // Generate inventory change event
            OnInventoryChange();

            return true;
        }

        return false;
    }

    /// <summary>
    ///     Removes all items of a given type from the player's inventory. This will the entry from the dictionary
    ///     as well as remove from the order list.
    /// </summary>
    /// <param name="itemType"> the item to remove from the player's inventory</param>
    /// 
    /// <returns>
    ///     On success, returns number of items remaining
    ///     If unsuccessful, return -1
    /// </returns>
    public int RemoveAllItemsOfType(InventoryItemType itemType)
    {
        int numberOfItemsInInventory = itemInventory.GetItemCount(itemType);
        int numberOfItemsRemaining = itemInventory.RemoveAllItemsOfType(itemType);

        // If it was successful in removing items from the inventory
        if (numberOfItemsRemaining != numberOfItemsInInventory)
        {
            ValidateInventory();

            OnInventoryChange();
        }

        return numberOfItemsInInventory;
    }

    public InventoryItemType? RemoveCurrentItem()
    {
        // Check if we have any items in our inventory
        if (inventoryOrder.Count == 0)
            return null;

        InventoryItemType currentItem = inventoryOrder[inventoryIndex];

        if (RemoveItem(currentItem))
            return currentItem;

        return null;
    }


    public int GetTotalItemCount()
    {
        int total = 0;
        foreach (InventoryItemType itemType in inventoryOrder)
        {
            total += itemInventory.GetItemCount(itemType);
        }

        return total;
    }

    public int GetUniqueItemCount()
    {
        return inventoryOrder.Count;
    }

    public int GetItemCount(InventoryItemType itemType)
    {
        return itemInventory.GetItemCount(itemType);
    }

    public int GetItemCount(int itemIndex)
    {
        bool indexIsValid = ValidateIndex(itemIndex);

        if (indexIsValid)
        {
            InventoryItemType itemType = inventoryOrder[itemIndex];
            return GetItemCount(itemType);
        }

        return 0;
    }

    ///   Current Item Methods
    public InventoryItemType? GetCurrentItemType()
    {
        if (inventoryOrder.Count > 0)
            return inventoryOrder[inventoryIndex];

        return null;
    }

    public GameObject GetCurrentItemGameObject()
    {
        return GetItemGameObject(inventoryIndex);
    }

    public GameObject GetCurrentItemPrefab()
    {
        return GetItemPrefab(inventoryIndex);
    }

    public Sprite GetCurrentItemImage()
    {
        return GetItemImage(inventoryIndex);
    }


    // Generic Get methods
    public GameObject GetItemGameObject(InventoryItemType itemType)
    {
        return itemInventory.GetItemGameObject(itemType);
    }

    public GameObject GetItemPrefab(InventoryItemType itemType)
    {
        return itemInventory.GetItemPrefab(itemType);
    }

    public Sprite GetItemImage(InventoryItemType itemType)
    {
        return itemInventory.GetItemImage(itemType);
    }

    public GameObject GetItemGameObject(int itemIndex)
    {
        bool indexIsValid = ValidateIndex(itemIndex);

        if (indexIsValid)
        {
            InventoryItemType itemType = inventoryOrder[itemIndex];
            return GetItemGameObject(itemType);
        }

        return null;
    }

    public GameObject GetItemPrefab(int itemIndex)
    {
        bool indexIsValid = ValidateIndex(itemIndex);

        if (indexIsValid)
        {
            InventoryItemType itemType = inventoryOrder[itemIndex];
            return GetItemPrefab(itemType);
        }

        return null;
    }

    public Sprite GetItemImage(int itemIndex)
    {
        bool indexIsValid = ValidateIndex(itemIndex);

        if (indexIsValid)
        {
            InventoryItemType itemType = inventoryOrder[itemIndex];
            return GetItemImage(itemType);
        }

        return null;
    }


    ///   Index Methods

    public int GetIndex()
    {
        return inventoryIndex;
    }

    public int IncrementIndex()
    {
        if (inventoryOrder.Count <= 0)
            return inventoryIndex;

        inventoryIndex = (inventoryIndex + 1) % inventoryOrder.Count;
        return inventoryIndex;
    }

    public int DecrementIndex()
    {
        if (inventoryIndex < 0)
            return inventoryIndex;

        inventoryIndex = inventoryIndex == 0 ? inventoryOrder.Count - 1 : inventoryIndex - 1;
        return inventoryIndex;
    }

    private void ValidateInventory()
    {
        // Our proper inventory will handle itself, we need to validate the inventoryOrder
        // We copy to array since we are removing items as we iterate over them
        InventoryItemType[] itemTypes = new InventoryItemType[inventoryOrder.Count];
        inventoryOrder.CopyTo(itemTypes);

        foreach(InventoryItemType itemType in itemTypes)
        {
            if (!itemInventory.ItemExistsInInventory(itemType))
            {
                inventoryOrder.Remove(itemType);
            }
        }

        // Make sure index is still valid
        if (inventoryIndex >= inventoryOrder.Count)
        {
            // Loop index back to beginning (ie next item)
            inventoryIndex = 0;
        }

        // Lastly check if we have any items
        if (inventoryOrder.Count == 0)
        {
            inventoryIndex = -1;
        }
        else if (inventoryIndex < 0)
        {
            inventoryIndex = 0;
        }
    }

    private bool ValidateIndex(int itemIndex)
    {
        if (itemIndex < 0)
            return false;
        else if (itemIndex >= inventoryOrder.Count)
            return false;

        // index is within range of the list, it must be valid
        return true;
    }

    // Method that will call/generate the InventoryChangeEvent
    private void OnInventoryChange()
    {
        if (InventoryChangeEvent != null)
        {
            InventoryChangeEvent(this);
        }
    }
}
