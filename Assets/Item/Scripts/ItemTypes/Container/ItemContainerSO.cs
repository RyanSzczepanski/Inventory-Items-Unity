using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Container", menuName = "Assets/Container")]
public class ItemContainerSO : ItemBasicSO, IContainerData
{
    public ContainerInventory containerInventory;

    public override ItemBasic CreateItem()
    {
        return new ItemContainer(this);
    }
}
