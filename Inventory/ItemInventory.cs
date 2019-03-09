using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class InventoryItemReference
{
    public InventoryItemType ItemType;
    public Queue<Pickup> PickupComps = new Queue<Pickup>();
    public PickupSO ItemPickupSO;
    public int ItemCount
    {
        get { return PickupComps.Count; }
    }
}

public class ItemInventory
{
    private Dictionary<InventoryItemType, InventoryItemReference> inventoryItemDictionary = new Dictionary<InventoryItemType, InventoryItemReference>();


    public bool ItemExistsInInventory(InventoryItemType itemType)
    {
        return inventoryItemDictionary.ContainsKey(itemType);
    }

    public int AddItem(Pickup pickup)
    {
        // Check if the itemType exists in the inventory
        if (!inventoryItemDictionary.ContainsKey(pickup.PickupObject.ItemType))
        {
            InventoryItemReference invItemRef = new InventoryItemReference();
            invItemRef.ItemType = pickup.PickupObject.ItemType;
            invItemRef.ItemPickupSO = pickup.PickupObject;

            inventoryItemDictionary.Add(pickup.PickupObject.ItemType, invItemRef);
        }

        // Increment the item count now that we're sure it exists in the inventory
        inventoryItemDictionary[pickup.PickupObject.ItemType].PickupComps.Enqueue(pickup);

        return inventoryItemDictionary[pickup.PickupObject.ItemType].ItemCount;
    }


    // Remove multiple items from the inventory
    // Returns how many items remain
    //   Returns -1 if it was unable to remove an item
    public int RemoveItem(InventoryItemType itemType, int numberToRemove)
    {
        // Check if itemType exists in dictionary and we have more than 0 of the given type
        if (inventoryItemDictionary.ContainsKey(itemType) && inventoryItemDictionary[itemType].ItemCount > 0 && numberToRemove > 0)
        {
            // Remove items
            int numberRemoved = 0;
            for (int i = numberToRemove; i > 0 && inventoryItemDictionary[itemType].PickupComps.Count > 0; --i)
            {
                ++numberRemoved;
                inventoryItemDictionary[itemType].PickupComps.Dequeue();
            }

            ValidateInventory();

            return numberRemoved;
        }
        else
        {
            return -1;
        }
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
        return RemoveItem(itemType, GetItemCount(itemType));
    }


    public int GetItemCount(InventoryItemType itemType)
    {
        if (inventoryItemDictionary.ContainsKey(itemType))
            return inventoryItemDictionary[itemType].ItemCount;

        return 0;
    }


    // Generic Get methods
    public GameObject GetItemGameObject(InventoryItemType itemType)
    {
        if (inventoryItemDictionary.ContainsKey(itemType))
        {
            Pickup pickup = inventoryItemDictionary[itemType].PickupComps.Peek();
            return pickup.gameObject;
        }

        return null;
    }

    public GameObject GetItemPrefab(InventoryItemType itemType)
    {
        if (inventoryItemDictionary.ContainsKey(itemType))
        {
            return inventoryItemDictionary[itemType].ItemPickupSO.GameObjectPrefab;
        }

        return null;
    }

    public Sprite GetItemImage(InventoryItemType itemType)
    {
        if (inventoryItemDictionary.ContainsKey(itemType))
        {
            return inventoryItemDictionary[itemType].ItemPickupSO.ItemImage;
        }

        return null;
    }


    private void ValidateInventory()
    {
        // Make sure that all items in inventory proper have more than 0 items
        InventoryItemType[] keyList = new InventoryItemType[inventoryItemDictionary.Count];
        inventoryItemDictionary.Keys.CopyTo(keyList, 0);
        foreach (InventoryItemType itemType in keyList)
        {
            if (inventoryItemDictionary[itemType].ItemCount <= 0)
            {
                inventoryItemDictionary.Remove(itemType);
            }
        }
    }
}
