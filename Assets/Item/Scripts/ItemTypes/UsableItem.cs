using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UseableItem : Item
{
    public int durability;
    public bool CanUse { get; }

    public UseableItem(UseableItemSO itemSO) : base(itemSO)
    {
        durability = itemSO.maxDurability;
    }

    public UseableItem(UseableItemSO itemSO, int _durability) :base(itemSO)
    {
        itemData = itemSO;
        durability = _durability;
    }

    public void Use(int useAmount)
    {
        if(useAmount > durability) { return; }
        durability -= useAmount;
        Debug.Log($"Used {useAmount} Units");
    }

    public void UseAll()
    {
        Use(durability);
    }
}
