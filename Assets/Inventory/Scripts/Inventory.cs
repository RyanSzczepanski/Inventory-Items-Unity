using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    public SubInventory[] SubInventories { get; private set; }

    public Inventory(IContainer container)
    {
        Vector2Int[] subInvs = container.Arrangement.SubInventories;
        SubInventories = new SubInventory[subInvs.Length];
        for (int i = 0; i < subInvs.Length; i++)
        {
            SubInventories[i] = new SubInventory(subInvs[i].x, subInvs[i].y, this);
        }
    }

    public Inventory(SubInventory[] subInventories)
    {
        this.SubInventories = subInventories;
    }

    public bool CanAddItem(ItemBasic item)
    {
        foreach (SubInventory subInventory in SubInventories)
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
        foreach (SubInventory subInventory in SubInventories)
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
