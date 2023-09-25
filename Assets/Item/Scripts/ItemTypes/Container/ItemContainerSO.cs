using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Container", menuName = "Assets/Container")]
public class ItemContainerSO : ItemBasicSO, IContainerData
{
    public ContainerInventory containerInventory;

    [field: SerializeField] public Arrangement Arrangement { get; private set; }

    public override ItemBasic CreateItem()
    {
        return new ItemContainer(this);
    }
}
