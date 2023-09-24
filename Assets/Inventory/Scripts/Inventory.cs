using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    public SubInventory[] subInventories;

    public Inventory()
    {

    }
    public Inventory(SubInventory[] subInventories)
    {
        this.subInventories = subInventories;
    }

    public bool CanAddItem(ItemBasic item)
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

    public SubInventory FindBestSubInventory(ItemBasic item)
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

    public void AddItem(ItemBasic item)
    {
        if (!CanAddItem(item))
        {
            Debug.Log($"Failed to add item: {item.data.name}");
            return;
        }
        SubInventory subInventory = FindBestSubInventory(item);
        subInventory.TryAddItem(item);
    }
}
