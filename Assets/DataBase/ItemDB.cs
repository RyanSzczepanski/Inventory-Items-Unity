using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDB : ScriptableObjectDatabase<ItemBasicSO>
{
    public static ItemBasicSO[] GetAllObjectsOfType(ItemType type)
    {
        List<ItemBasicSO> items = new List<ItemBasicSO>();
        foreach (ItemBasicSO item in GetValues())
        {
            if (item.type == type)
            {
                items.Add(item);
            }
        }
        return items.ToArray();
    }
}
