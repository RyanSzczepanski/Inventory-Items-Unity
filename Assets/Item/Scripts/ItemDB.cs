using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDB : ScriptableObjectDatabase<ItemSO>
{
    public static ItemSO[] GetAllObjectsOfType(ItemType type)
    {
        List<ItemSO> items = new List<ItemSO>();
        foreach (ItemSO item in GetValues())
        {
            if (item.type == type)
            {
                items.Add(item);
            }
        }
        return items.ToArray();
    }
}
