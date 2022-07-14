using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    public SubInventory[] subInventories;

    public Inventory(SubInventory[] subInventories)
    {
        this.subInventories = subInventories;
    }

    public bool CanAddItem(Item item)
    {
        foreach (SubInventory subInventory in subInventories)
        {
            if (subInventory.CanAddItem(item))
            {
                return true;
            }
        }
        return false;
    }

    public SubInventory FindBestSubInventory(Item item)
    {
        foreach(SubInventory subInventory in subInventories)
        {
            if (subInventory.CanAddItem(item))
            {
                return subInventory;
            }
        }
        return null;
    }

    public void AddItem(Item item)
    {
        if (!CanAddItem(item))
        {
            Debug.Log($"Failed to add item: {item.itemData.name}");
            return;
        }
        SubInventory subInventory = FindBestSubInventory(item);
        subInventory.TryAddItem(item);
    }
}
