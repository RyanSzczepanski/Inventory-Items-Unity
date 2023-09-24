using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Container", menuName = "Assets/Container")]
public class ItemContainerSO : ItemBasicSO, IContainerData
{
    //public Vector2Int[] miniInventories;
    public ContainerInventory containerInventory;

    public override ItemBasic CreateItem()
    {
        return new ItemContainer(this);
    }
}
