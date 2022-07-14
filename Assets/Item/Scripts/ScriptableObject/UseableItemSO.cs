using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Usable Item", menuName = "Assets/UsableItem")]
public class UseableItemSO : ItemSO
{
    public int maxDurability;

    public override Item CreateItem()
    {
        return new UseableItem(this);
    }

    public override void AddItemScript(GameObject gameObject)
    {
        //UsableItem usableItem = gameObject.AddComponent<UsableItem>();
        //usableItem.durability = maxDurability;
        //usableItem.itemData = this;
    }
}
