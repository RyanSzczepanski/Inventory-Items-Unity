using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Container", menuName = "Assets/Container")]
public class ContainerItemSO : ItemSO
{
    //public Vector2Int[] miniInventories;
    public ContainerInventory containerInventory;

    public override Item CreateItem()
    {
        return new ContainerItem(this);
    }
}
